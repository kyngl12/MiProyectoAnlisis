using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.ObtenerTodosLosReportesMensuales;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ReportesMensuales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.AccesoADatos.ReportesMensuales.ObtenerTodosLosReportesMensuales
{
    public class ObtenerTodosLosReportesMensualesAD : IObtenerTodosLosReportesMensualesAD
    {
        Contexto _elContexto;

        public ObtenerTodosLosReportesMensualesAD()
        {
            _elContexto = new Contexto();
        }

        public List<ReportesMensualesDto> Obtener()
        {
            List<ReportesMensualesDto> laListaDeReportes = (from elReporte in _elContexto.ReportesMensuales
                                                            join elComercio in _elContexto.Comercios
                                                            on elReporte.IdComercio equals elComercio.IdComercio
                                                            select new ReportesMensualesDto
                                                            {
                                                                IdReporte = elReporte.IdReporte,
                                                                IdComercio = elReporte.IdComercio,
                                                                NombreComercio = elComercio.Nombre,
                                                                CantidadDeCajas = elReporte.CantidadDeCajas,
                                                                MontoTotalRecaudado = elReporte.MontoTotalRecaudado,
                                                                CantidadDeSINPES = elReporte.CantidadDeSINPES,
                                                                MontoTotalComision = elReporte.MontoTotalComision,
                                                                FechaDelReporte = elReporte.FechaDelReporte
                                                            }).ToList();

            return laListaDeReportes;
        }
    }
}
