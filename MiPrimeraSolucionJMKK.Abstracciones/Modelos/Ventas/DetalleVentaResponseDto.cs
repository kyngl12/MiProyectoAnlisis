namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas
{
    /// <summary>
    /// GPV-005: linea de detalle de una venta para mostrar al usuario.
    /// </summary>
    public class DetalleVentaResponseDto
    {
        public int IdDetalleVenta { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
