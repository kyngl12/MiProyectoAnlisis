using System;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes
{
    public class ReporteExportRowDto
    {
        public string NumeroFactura { get; set; }

        public DateTime Fecha { get; set; }

        public string Cliente { get; set; }

        public string Usuario { get; set; }

        public string MetodoPago { get; set; }

        public decimal Subtotal { get; set; }

        public decimal IVA { get; set; }

        public decimal Descuento { get; set; }

        public decimal Total { get; set; }

        public string Estado { get; set; }
    }
}
