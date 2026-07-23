namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas
{
    /// <summary>
    /// GPV-004: resultado de busqueda de productos activos con stock disponible.
    /// </summary>
    public class BuscarProductoResponseDto
    {
        public int IdProducto { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int StockDisponible { get; set; }
        public string Categoria { get; set; }
    }
}
