using System;
using System.Collections.Generic;
using System.Linq;
using GestionPubRock.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.BitacoraContable;
using GestionPubRock.LogicaDeNegocio.Bitacora.BitacoraContable;

namespace GestionPubRock.LogicaDeNegocio.Contabilidad
{
    public class ObtenerImpuestosLN : IObtenerImpuestosLN
    {
        private readonly IObtenerImpuestosAD _obtenerImpuestosAD;

        public ObtenerImpuestosLN() : this(new ObtenerImpuestosAD()) { }

        public ObtenerImpuestosLN(IObtenerImpuestosAD obtenerImpuestosAD)
        {
            _obtenerImpuestosAD = obtenerImpuestosAD;
        }

        public List<ImpuestoDto> Obtener()
        {
            return _obtenerImpuestosAD.Obtener();
        }
    }

    public class ActualizarImpuestoLN : IActualizarImpuestoLN
    {
        private readonly IActualizarImpuestoAD _actualizarImpuestoAD;
        private readonly IRegistrarBitacoraContableLN _bitacora;

        public ActualizarImpuestoLN() : this(new ActualizarImpuestoAD(), new RegistrarBitacoraContableLN()) { }

        public ActualizarImpuestoLN(IActualizarImpuestoAD actualizarImpuestoAD, IRegistrarBitacoraContableLN bitacora)
        {
            _actualizarImpuestoAD = actualizarImpuestoAD;
            _bitacora = bitacora;
        }

        public bool Actualizar(ImpuestoDto impuesto)
        {
            try
            {
                if (impuesto == null || impuesto.IdImpuesto <= 0)
                    throw new ArgumentException("El impuesto indicado no es válido.");

                // Escenario 4: porcentaje negativo, mayor a 100 o no numerico.
                if (impuesto.Porcentaje < 0 || impuesto.Porcentaje > 100)
                    throw new ArgumentException("El valor ingresado no es válido. Las tasas deben ser porcentajes entre 0 y 100.");

                bool actualizado = _actualizarImpuestoAD.Actualizar(impuesto);

                if (!actualizado)
                    throw new ArgumentException("El impuesto indicado no se encuentra registrado.");

                _bitacora.Registrar(
                    "Configuración de Impuestos",
                    "Se actualizó la tasa de " + impuesto.NombreImpuesto + " a " + impuesto.Porcentaje + "%",
                    impuesto.ActualizadoPor,
                    "Contabilidad");

                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    System.IO.File.AppendAllText(
                        AppDomain.CurrentDomain.BaseDirectory + "App_Data\\errors.log",
                        DateTime.Now.ToString("s") + " - " + ex.ToString() + Environment.NewLine);
                }
                catch { }

                throw;
            }
        }
    }

    public class CalcularImpuestosLN : ICalcularImpuestosLN
    {
        private readonly IObtenerImpuestosLN _obtenerImpuestosLN;

        public CalcularImpuestosLN() : this(new ObtenerImpuestosLN()) { }

        public CalcularImpuestosLN(IObtenerImpuestosLN obtenerImpuestosLN)
        {
            _obtenerImpuestosLN = obtenerImpuestosLN;
        }

        public CalculoImpuestoDto Calcular(decimal subtotal, bool exento)
        {
            if (subtotal < 0)
                throw new ArgumentException("El subtotal de la orden no puede ser negativo.");

            var impuestos = _obtenerImpuestosLN.Obtener();

            decimal porcentajeIva = impuestos
                .Where(i => i.NombreImpuesto.Equals("IVA", StringComparison.OrdinalIgnoreCase))
                .Select(i => i.Porcentaje)
                .DefaultIfEmpty(13.00m)
                .First();

            decimal porcentajeServicio = impuestos
                .Where(i => i.NombreImpuesto.Equals("Servicio", StringComparison.OrdinalIgnoreCase))
                .Select(i => i.Porcentaje)
                .DefaultIfEmpty(10.00m)
                .First();

            var resultado = new CalculoImpuestoDto
            {
                Subtotal = subtotal,
                Exento = exento,
                PorcentajeIva = porcentajeIva,
                PorcentajeServicio = porcentajeServicio
            };

            if (exento)
            {
                // Escenario 3: factura exenta, el impuesto queda en cero.
                resultado.MontoIva = 0;
                resultado.MontoServicio = 0;
            }
            else
            {
                // Escenario 1: calculo automatico del 13% de IVA y 10% de servicio.
                resultado.MontoIva = Math.Round(subtotal * porcentajeIva / 100m, 2);
                resultado.MontoServicio = Math.Round(subtotal * porcentajeServicio / 100m, 2);
            }

            resultado.Total = subtotal + resultado.MontoIva + resultado.MontoServicio;

            return resultado;
        }
    }
}
