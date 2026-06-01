using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.ObtenerTodosLosReportesMensuales;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ReportesMensuales;
using MiPrimeraSolucionJMKK.AccesoADatos.ReportesMensuales.ObtenerTodosLosReportesMensuales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

