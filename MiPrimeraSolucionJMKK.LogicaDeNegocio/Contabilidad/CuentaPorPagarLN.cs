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
    public class RegistrarCuentaPorPagarLN : IRegistrarCuentaPorPagarLN
    {
        private readonly IRegistrarCuentaPorPagarAD _registrarCuentaPorPagarAD;
        private readonly IRegistrarBitacoraContableLN _bitacora;

        public RegistrarCuentaPorPagarLN() : this(new RegistrarCuentaPorPagarAD(), new RegistrarBitacoraContableLN()) { }

        public RegistrarCuentaPorPagarLN(IRegistrarCuentaPorPagarAD registrarCuentaPorPagarAD, IRegistrarBitacoraContableLN bitacora)
        {
            _registrarCuentaPorPagarAD = registrarCuentaPorPagarAD;
            _bitacora = bitacora;
        }

        public bool Registrar(CuentaPorPagarDto cuenta)
        {
            try
            {
                if (cuenta == null)
                    throw new ArgumentException("Debe indicar los datos de la cuenta por pagar.");

                // Escenario 5: campos obligatorios en blanco.
                var camposFaltantes = new List<string>();

                if (string.IsNullOrWhiteSpace(cuenta.Proveedor))
                    camposFaltantes.Add("Proveedor");

                if (cuenta.Monto <= 0)
                    camposFaltantes.Add("Monto");

                if (string.IsNullOrWhiteSpace(cuenta.Descripcion))
                    camposFaltantes.Add("Descripción");

                if (cuenta.FechaVencimiento == default(DateTime))
                    camposFaltantes.Add("Fecha de Vencimiento");

                if (camposFaltantes.Count > 0)
                    throw new ArgumentException(
                        "Los siguientes campos son obligatorios y deben completarse: " +
                        string.Join(", ", camposFaltantes));

                int resultado = _registrarCuentaPorPagarAD.Registrar(cuenta);

                if (resultado > 0)
                {
                    _bitacora.Registrar(
                        "Registro de Cuenta por Pagar",
                        "Se registró una cuenta por pagar a " + cuenta.Proveedor + " por " + cuenta.Monto.ToString("C2") +
                        ", vence el " + cuenta.FechaVencimiento.ToString("dd/MM/yyyy"),
                        cuenta.RegistradoPor,
                        "Contabilidad");

                    return true;
                }

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

    public class ObtenerCuentasPorPagarLN : IObtenerCuentasPorPagarLN
    {
        private readonly IObtenerCuentasPorPagarAD _obtenerCuentasPorPagarAD;

        public ObtenerCuentasPorPagarLN() : this(new ObtenerCuentasPorPagarAD()) { }

        public ObtenerCuentasPorPagarLN(IObtenerCuentasPorPagarAD obtenerCuentasPorPagarAD)
        {
            _obtenerCuentasPorPagarAD = obtenerCuentasPorPagarAD;
        }

        public List<CuentaPorPagarDto> Obtener()
        {
            return _obtenerCuentasPorPagarAD.Obtener();
        }
    }

    public class MarcarCuentaPorPagarComoPagadaLN : IMarcarCuentaPorPagarComoPagadaLN
    {
        private readonly IMarcarCuentaPorPagarComoPagadaAD _marcarCuentaPorPagarComoPagadaAD;
        private readonly IRegistrarBitacoraContableLN _bitacora;

        public MarcarCuentaPorPagarComoPagadaLN() : this(new MarcarCuentaPorPagarComoPagadaAD(), new RegistrarBitacoraContableLN()) { }

        public MarcarCuentaPorPagarComoPagadaLN(IMarcarCuentaPorPagarComoPagadaAD marcarCuentaPorPagarComoPagadaAD, IRegistrarBitacoraContableLN bitacora)
        {
            _marcarCuentaPorPagarComoPagadaAD = marcarCuentaPorPagarComoPagadaAD;
            _bitacora = bitacora;
        }

        public bool MarcarComoPagada(int idCuentaPorPagar, string cedulaRegistro)
        {
            try
            {
                if (idCuentaPorPagar <= 0)
                    throw new ArgumentException("La cuenta por pagar indicada no es válida.");

                int resultado = _marcarCuentaPorPagarComoPagadaAD.MarcarComoPagada(idCuentaPorPagar, cedulaRegistro);

                if (resultado > 0)
                {
                    _bitacora.Registrar(
                        "Liquidación de Cuenta por Pagar",
                        "Se liquidó la cuenta por pagar #" + idCuentaPorPagar,
                        cedulaRegistro,
                        "Contabilidad");

                    return true;
                }

                if (resultado == -1)
                    throw new ArgumentException("La cuenta por pagar no se encuentra registrada.");

                if (resultado == -2)
                    throw new InvalidOperationException("La cuenta por pagar ya se encuentra liquidada.");

                if (resultado == -3)
                    throw new ArgumentException("No fue posible identificar al empleado que realiza el pago.");

                throw new ApplicationException("Error en el sistema. Favor intente de nuevo.");
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
}
