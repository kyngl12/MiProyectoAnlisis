using System;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes
{
    public class ReporteInventarioRowDto
    {
        public int? Codigo { get; set; }
        public string Producto { get; set; }
        public int? Categoria { get; set; }
        public int? StockActual { get; set; }
        public int? StockMinimo { get; set; }
        public int? StockMaximo { get; set; }
        public decimal? Precio { get; set; }
        public string EstadoInventario { get; set; }
    }
}
