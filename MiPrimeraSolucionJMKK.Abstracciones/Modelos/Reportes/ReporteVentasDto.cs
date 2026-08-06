using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes
{
    public class ReporteVentasDto
    {
        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public int CantidadTransacciones { get; set; }

        public decimal MontoFinalRecaudado { get; set; }

        public List<MetodoPagoReporteDto> MetodosPago { get; set; }

        public List<DetalleVentaReporteDto> Ventas { get; set; }

        public ReporteVentasDto()
        {
            MetodosPago = new List<MetodoPagoReporteDto>();
            Ventas = new List<DetalleVentaReporteDto>();
        }
    }
}
