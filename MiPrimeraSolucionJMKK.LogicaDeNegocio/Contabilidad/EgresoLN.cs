using System;
using System.Collections.Generic;
using GestionPubRock.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.BitacoraContable;
using GestionPubRock.LogicaDeNegocio.Bitacora.BitacoraContable;

namespace GestionPubRock.LogicaDeNegocio.Contabilidad
{
    public class RegistrarEgresoLN : IRegistrarEgresoLN
    {
        private readonly IRegistrarEgresoAD _registrarEgresoAD;
        private readonly IRegistrarBitacoraContableLN _bitacora;

        public RegistrarEgresoLN() : this(new RegistrarEgresoAD(), new RegistrarBitacoraContableLN()) { }

        public RegistrarEgresoLN(IRegistrarEgresoAD registrarEgresoAD, IRegistrarBitacoraContableLN bitacora)
        {
            _registrarEgresoAD = registrarEgresoAD;
            _bitacora = bitacora;
        }

        public bool Registrar(EgresoDto egreso)
        {
            try
            {
                if (egreso == null)
                    throw new ArgumentException("Debe indicar los datos del gasto.");

                // Escenario 2: campos obligatorios en blanco.
                var camposFaltantes = new List<string>();

                if (egreso.Monto == 0)
                    camposFaltantes.Add("Monto");

                if (string.IsNullOrWhiteSpace(egreso.Descripcion))
                    camposFaltantes.Add("Descripción");

                if (string.IsNullOrWhiteSpace(egreso.Categoria))
                    camposFaltantes.Add("Categoría");

                if (egreso.Fecha == default(DateTime))
                    camposFaltantes.Add("Fecha");

                if (camposFaltantes.Count > 0)
                    throw new ArgumentException(
                        "Los siguientes campos obligatorios deben completarse: " +
                        string.Join(", ", camposFaltantes));

                // Escenario 3: el monto debe ser un valor mayor a cero.
                if (egreso.Monto <= 0)
                    throw new ArgumentException("El monto ingresado no es válido. Debe ser un valor mayor a cero.");

                int resultado = _registrarEgresoAD.Registrar(egreso);

                if (resultado > 0)
                {
                    _bitacora.Registrar(
                        "Registro de Egreso",
                        "Se registró un egreso de " + egreso.Monto.ToString("C2") + " en la categoría " + egreso.Categoria + ": " + egreso.Descripcion,
                        egreso.RegistradoPor,
                        "Contabilidad");

                    return true;
                }

                if (resultado == -3)
                    throw new ArgumentException("No fue posible identificar al empleado que registra el gasto.");

                // Escenario 5: error interno del sistema.
                throw new ApplicationException("Error en el sistema. Favor intente de nuevo.");
            }
            catch (Exception ex)
            {
                RegistrarErrorEnArchivo(ex);
                throw;
            }
        }

        private void RegistrarErrorEnArchivo(Exception ex)
        {
            try
            {
                System.IO.File.AppendAllText(
                    AppDomain.CurrentDomain.BaseDirectory + "App_Data\\errors.log",
                    DateTime.Now.ToString("s") + " - " + ex.ToString() + Environment.NewLine);
            }
            catch { }
        }
    }

    public class ObtenerEgresosLN : IObtenerEgresosLN
    {
        private readonly IObtenerEgresosAD _obtenerEgresosAD;

        public ObtenerEgresosLN() : this(new ObtenerEgresosAD()) { }

        public ObtenerEgresosLN(IObtenerEgresosAD obtenerEgresosAD)
        {
            _obtenerEgresosAD = obtenerEgresosAD;
        }

        public List<EgresoDto> Obtener(DateTime? fechaInicio, DateTime? fechaFin)
        {
            return _obtenerEgresosAD.Obtener(fechaInicio, fechaFin);
        }
    }
}
