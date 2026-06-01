using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ReportesMensuales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.GenerarReporteMensual
{
    public interface IGenerarReportesMensualesAD
    {
        ReportesMensualesDto ObtenerReporteMensualExistente(int idComercio, int mes, int anio);
        int RegistrarReporteMensual(ReportesMensualesDto elReporte);
        int ActualizarReporteMensual(ReportesMensualesDto elReporte);
    }
}
