using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ReportesMensuales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.ObtenerTodosLosReportesMensuales
{
    public interface IObtenerTodosLosReportesMensualesAD
    {
        List<ReportesMensualesDto> Obtener();
    }
}
