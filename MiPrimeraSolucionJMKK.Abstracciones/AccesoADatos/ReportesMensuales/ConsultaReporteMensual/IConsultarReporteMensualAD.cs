using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.ConsultaReporteMensual
{
    public interface IConsultarReporteMensualAD
    {
        List<ComercioDto> ObtenerComercios();
        int ObtenerCantidadDeCajas(int idComercio);
        List<string> ObtenerTelefonosDeCajas(int idComercio);
        List<SINPESDto> ObtenerSINPESDelMes(List<string> telefonosDeCajas, int mes, int anio);
        int ObtenerComision(int idComercio);
    }
}
