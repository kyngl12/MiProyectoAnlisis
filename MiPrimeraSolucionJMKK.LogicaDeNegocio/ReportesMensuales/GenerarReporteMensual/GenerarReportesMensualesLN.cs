using GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora;
using GestionPubRock.AccesoADatos.ReportesMensuales.ConsultarReporteMensual;
using GestionPubRock.AccesoADatos.ReportesMensuales.GenerarReporteMensual;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.ConsultaReporteMensual;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.GenerarReporteMensual;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ReportesMensuales;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.ReportesMensuales.ObtenerTodosLosReportesMensuales
{
    public class GenerarReportesMensualesLN : IGenerarReportesMensualesLN
    {
        IGenerarReportesMensualesAD _generarReportesMensualesAD;
        IConsultarReporteMensualAD _consultarReporteMensualAD;
        IRegistrarBitacoraLN _bitacora;

        public GenerarReportesMensualesLN()
        {
            _generarReportesMensualesAD = new GenerarReportesMensualesAD();
            _consultarReporteMensualAD = new ConsultarReporteMensualAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public bool GenerarReportesMensuales()
        {
            try
            {
                int cantidadDeCambios = 0;
                int mesActual = DateTime.Now.Month;
                int anioActual = DateTime.Now.Year;
                DateTime fechaActual = DateTime.Now;

                List<ComercioDto> laListaDeComercios = _consultarReporteMensualAD.ObtenerComercios();

                foreach (ComercioDto elComercio in laListaDeComercios)
                {
                    int cantidadDeCajas = _consultarReporteMensualAD.ObtenerCantidadDeCajas(elComercio.IdComercio);

                    List<string> telefonosDeCajas = _consultarReporteMensualAD.ObtenerTelefonosDeCajas(elComercio.IdComercio);

                    List<SINPESDto> losSINPESDelMes = _consultarReporteMensualAD.ObtenerSINPESDelMes(telefonosDeCajas, mesActual, anioActual);

                    decimal montoTotalRecaudado = 0;

                    foreach (SINPESDto elSINPE in losSINPESDelMes)
                    {
                        montoTotalRecaudado = montoTotalRecaudado + elSINPE.Monto;
                    }

                    int cantidadDeSINPES = losSINPESDelMes.Count;

                    int comision = _consultarReporteMensualAD.ObtenerComision(elComercio.IdComercio);

                    decimal porcentajeDeComision = (decimal)comision / 100;
                    decimal montoTotalComision = montoTotalRecaudado * porcentajeDeComision;

                    ReportesMensualesDto elReporteAGuardar = new ReportesMensualesDto
                    {
                        IdComercio = elComercio.IdComercio,
                        NombreComercio = elComercio.Nombre,
                        CantidadDeCajas = cantidadDeCajas,
                        MontoTotalRecaudado = montoTotalRecaudado,
                        CantidadDeSINPES = cantidadDeSINPES,
                        MontoTotalComision = montoTotalComision,
                        FechaDelReporte = fechaActual
                    };

                    ReportesMensualesDto reporteExistente = _generarReportesMensualesAD
                        .ObtenerReporteMensualExistente(elComercio.IdComercio, mesActual, anioActual);

                    if (reporteExistente == null)
                    {
                        int cantidad = _generarReportesMensualesAD.RegistrarReporteMensual(elReporteAGuardar);

                        if (cantidad > 0)
                        {
                            _bitacora.Registrar("REPORTES_MENSUALES", "INSERT", null, elReporteAGuardar);
                            cantidadDeCambios = cantidadDeCambios + cantidad;
                        }
                    }
                    else
                    {
                        elReporteAGuardar.IdReporte = reporteExistente.IdReporte;

                        int cantidad = _generarReportesMensualesAD.ActualizarReporteMensual(elReporteAGuardar);

                        if (cantidad > 0)
                        {
                            _bitacora.Registrar("REPORTES_MENSUALES", "UPDATE", reporteExistente, elReporteAGuardar);
                            cantidadDeCambios = cantidadDeCambios + cantidad;
                        }
                    }
                }

                return cantidadDeCambios > 0;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("REPORTES_MENSUALES", ex);
                throw;
            }
        }
    }
}
