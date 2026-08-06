using GestionPubRock.AccesoADatos.Reportes;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Reportes;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.LogicaDeNegocio.Reportes
{
    public class ReporteVentasLN : IReporteVentasLN
    {
        private readonly ReporteVentasAD _reporteVentasAD;

        public ReporteVentasLN()
        {
            _reporteVentasAD = new ReporteVentasAD();
        }

        public ReporteVentasDto Generar(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio.Date > fechaFin.Date)
            {
                throw new ArgumentException(
                    "La fecha de inicio no puede ser mayor a la fecha de finalización"
                );
            }

            try
            {
                ReporteVentasDto reporte =
                    _reporteVentasAD.Generar(fechaInicio, fechaFin);

                if (reporte == null ||
                    reporte.Ventas == null ||
                    reporte.Ventas.Count == 0)
                {
                    throw new InvalidOperationException(
                        "No existen registros de ventas para el período seleccionado"
                    );
                }

                return reporte;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception(
                    "Ocurrió un problema al generar el reporte. Por favor intente nuevamente."
                );
            }
        }

        public ReporteVentasDto GenerarFiltrado(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado)
        {
            if (fechaInicio.HasValue && fechaFin.HasValue && fechaInicio.Value.Date > fechaFin.Value.Date)
            {
                throw new ArgumentException(
                    "La fecha de inicio no puede ser mayor a la fecha de finalización"
                );
            }

            try
            {
                var reporte = _reporteVentasAD.GenerarFiltrado(fechaInicio, fechaFin, idCategoria, idTipoPago, idCliente, idEstado);

                if (reporte == null || reporte.Ventas == null || reporte.Ventas.Count == 0)
                {
                    throw new InvalidOperationException(
                        "No existen registros de ventas para los filtros seleccionados"
                    );
                }

                return reporte;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new Exception(
                    "Ocurrió un problema al generar el reporte filtrado. Por favor intente nuevamente."
                );
            }
        }
    }
}
