using System;
using System.Globalization;
using GestionPubRock.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.BitacoraContable;
using GestionPubRock.LogicaDeNegocio.Bitacora.BitacoraContable;

namespace GestionPubRock.LogicaDeNegocio.Contabilidad
{
    public class RealizarCierreCajaLN : IRealizarCierreCajaLN
    {
        private readonly IObtenerResumenDelDiaAD _obtenerResumenDelDiaAD;
        private readonly IObtenerCierreCajaPorFechaAD _obtenerCierreCajaPorFechaAD;
        private readonly IRegistrarCierreCajaAD _registrarCierreCajaAD;
        private readonly IRegistrarBitacoraContableLN _bitacora;

        public RealizarCierreCajaLN()
            : this(new ObtenerResumenDelDiaAD(), new ObtenerCierreCajaPorFechaAD(),
                   new RegistrarCierreCajaAD(), new RegistrarBitacoraContableLN())
        {
        }

        public RealizarCierreCajaLN(
            IObtenerResumenDelDiaAD obtenerResumenDelDiaAD,
            IObtenerCierreCajaPorFechaAD obtenerCierreCajaPorFechaAD,
            IRegistrarCierreCajaAD registrarCierreCajaAD,
            IRegistrarBitacoraContableLN bitacora)
        {
            _obtenerResumenDelDiaAD = obtenerResumenDelDiaAD;
            _obtenerCierreCajaPorFechaAD = obtenerCierreCajaPorFechaAD;
            _registrarCierreCajaAD = registrarCierreCajaAD;
            _bitacora = bitacora;
        }

        public CierreCajaDto ObtenerResumenDelDia(DateTime fecha)
        {
            return _obtenerResumenDelDiaAD.ObtenerResumen(fecha);
        }

        public CierreCajaDto Cerrar(DateTime fecha, decimal montoContado, string cedulaRegistro)
        {
            try
            {
                if (montoContado < 0)
                    throw new ArgumentException("El monto contado debe ser mayor o igual a cero.");

                // Escenario 4: cierre duplicado.
                var cierreExistente = _obtenerCierreCajaPorFechaAD.ObtenerPorFecha(fecha);
                if (cierreExistente != null)
                    throw new InvalidOperationException("La caja ya se encuentra cerrada para esta jornada.");

                var resumen = _obtenerResumenDelDiaAD.ObtenerResumen(fecha);

                // Escenario 5: sin ventas registradas en la jornada.
                if (resumen.TotalVentas == 0)
                    throw new InvalidOperationException("No existen ventas registradas para esta jornada.");

                resumen.MontoContado = montoContado;
                resumen.Diferencia = montoContado - resumen.BalanceFinal;
                resumen.RegistradoPor = cedulaRegistro;

                if (resumen.Diferencia == 0)
                {
                    // Escenario 1: cierre cuadrado.
                    resumen.ResultadoCuadre = "Cierre exitoso: La caja cuadra perfectamente";
                }
                else if (resumen.Diferencia < 0)
                {
                    // Escenario 2: faltante de dinero.
                    resumen.ResultadoCuadre = "Atención: Existe un descuadre de " +
                        Math.Abs(resumen.Diferencia).ToString("C2", CultureInfo.GetCultureInfo("es-CR"));
                }
                else
                {
                    // Escenario 3: sobrante de dinero.
                    resumen.ResultadoCuadre = "Atención: Existe un excedente de " +
                        resumen.Diferencia.ToString("C2", CultureInfo.GetCultureInfo("es-CR"));
                }

                int idCierre = _registrarCierreCajaAD.Registrar(resumen);

                if (idCierre == -1)
                    throw new InvalidOperationException("La caja ya se encuentra cerrada para esta jornada.");

                if (idCierre == -3)
                    throw new ArgumentException("No fue posible identificar al empleado que realiza el cierre.");

                if (idCierre <= 0)
                    throw new ApplicationException("Error en el sistema. Favor intente de nuevo.");

                _bitacora.Registrar(
                    "Cierre de Caja",
                    resumen.ResultadoCuadre + " (diferencia: " + resumen.Diferencia.ToString("C2", CultureInfo.GetCultureInfo("es-CR")) + ")",
                    cedulaRegistro,
                    "Contabilidad");

                return resumen;
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
}
