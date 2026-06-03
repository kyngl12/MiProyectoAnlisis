using GestionPubRock.AccesoADatos.ReportesMensuales.ObtenerTodosLosReportesMensuales;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.ObtenerTodosLosReportesMensuales;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ReportesMensuales;
using System.Collections.Generic;
using System.Linq;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.ReportesMensuales.GenerarReporteMensual
{
    public class ObtenerTodosLosReportesMensualesLN : IObtenerTodosLosReportesMensualesLN
    {
        IObtenerTodosLosReportesMensualesAD _obtenerTodosLosReportesMensualesAD;

        public ObtenerTodosLosReportesMensualesLN()
        {
            _obtenerTodosLosReportesMensualesAD = new ObtenerTodosLosReportesMensualesAD();
        }

        public List<ReportesMensualesDto> Obtener()
        {
            List<ReportesMensualesDto> laListaDeReportes = _obtenerTodosLosReportesMensualesAD.Obtener();

            laListaDeReportes = laListaDeReportes
                .ToList();

            return laListaDeReportes;
        }
    }
}

