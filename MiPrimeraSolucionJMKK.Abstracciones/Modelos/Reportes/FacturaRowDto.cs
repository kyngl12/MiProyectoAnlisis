using System;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes
{
    public class FacturaRowDto
    {
        public int IdFactura { get; set; }
        public DateTime? FechaFactura { get; set; }
        public decimal? MontoTotal { get; set; }
        public int? IdTipoPago { get; set; }
        public int? IdEstado { get; set; }
        public int? IdVenta { get; set; }
    }
}
