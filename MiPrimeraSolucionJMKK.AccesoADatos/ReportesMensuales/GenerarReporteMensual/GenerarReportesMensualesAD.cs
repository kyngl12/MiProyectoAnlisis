using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.GenerarReporteMensual;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ReportesMensuales;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.AccesoADatos.ReportesMensuales.GenerarReporteMensual
{
    public class GenerarReportesMensualesAD : IGenerarReportesMensualesAD
    {
        Contexto _elContexto;

        public GenerarReportesMensualesAD()
        {
            _elContexto = new Contexto();
        }

        public ReportesMensualesDto ObtenerReporteMensualExistente(int idComercio, int mes, int anio)
        {
            ReportesMensualesDto reporteExistente = (from r in _elContexto.ReportesMensuales
                                                  where r.IdComercio == idComercio
                                                     && r.FechaDelReporte.Month == mes
                                                     && r.FechaDelReporte.Year == anio
                                                  select new ReportesMensualesDto
                                                  {
                                                      IdReporte = r.IdReporte,
                                                      IdComercio = r.IdComercio,
                                                      NombreComercio = r.NombreComercio,
                                                      CantidadDeCajas = r.CantidadDeCajas,
                                                      MontoTotalRecaudado = r.MontoTotalRecaudado,
                                                      CantidadDeSINPES = r.CantidadDeSINPES,
                                                      MontoTotalComision = r.MontoTotalComision,
                                                      FechaDelReporte = r.FechaDelReporte
                                                  }).FirstOrDefault();

            return reporteExistente;
        }

        public int RegistrarReporteMensual(ReportesMensualesDto elReporte)
        {
            ReportesMensualesEntidad nuevoReporte = ConvertirObjetoEntidad(elReporte);

            _elContexto.ReportesMensuales.Add(nuevoReporte);

            int cantidadDeRegistrosAlmacenados = _elContexto.SaveChanges();

            return cantidadDeRegistrosAlmacenados;
        }

        public int ActualizarReporteMensual(ReportesMensualesDto elReporte)
        {
            ReportesMensualesEntidad reporteExistente = _elContexto.ReportesMensuales
                .Where(r => r.IdReporte == elReporte.IdReporte)
                .FirstOrDefault();

            if (reporteExistente == null)
            {
                return 0;
            }

            reporteExistente.NombreComercio = elReporte.NombreComercio;
            reporteExistente.CantidadDeCajas = elReporte.CantidadDeCajas;
            reporteExistente.MontoTotalRecaudado = elReporte.MontoTotalRecaudado;
            reporteExistente.CantidadDeSINPES = elReporte.CantidadDeSINPES;
            reporteExistente.MontoTotalComision = elReporte.MontoTotalComision;
            reporteExistente.FechaDelReporte = elReporte.FechaDelReporte;

            int cantidadDeRegistrosAlmacenados = _elContexto.SaveChanges();

            return cantidadDeRegistrosAlmacenados;
        }

        private ReportesMensualesEntidad ConvertirObjetoEntidad(ReportesMensualesDto elReporte)
        {
            return new ReportesMensualesEntidad
            {
                IdComercio = elReporte.IdComercio,
                NombreComercio = elReporte.NombreComercio,
                CantidadDeCajas = elReporte.CantidadDeCajas,
                MontoTotalRecaudado = elReporte.MontoTotalRecaudado,
                CantidadDeSINPES = elReporte.CantidadDeSINPES,
                MontoTotalComision = elReporte.MontoTotalComision,
                FechaDelReporte = elReporte.FechaDelReporte
            };
        }
    }
}
