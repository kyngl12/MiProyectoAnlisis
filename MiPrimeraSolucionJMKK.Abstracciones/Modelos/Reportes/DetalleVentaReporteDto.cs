using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes
{
    public class DetalleVentaReporteDto
    {
        public int IdVenta { get; set; }

        public DateTime FechaVenta { get; set; }

        public string NumeroFactura { get; set; }

        public string MetodoPago { get; set; }

        public decimal TotalVenta { get; set; }
    }
}
