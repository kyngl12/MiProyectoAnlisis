namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas
{
    /// <summary>
    /// GPV-001: producto y cantidad que se agregan a la venta.
    /// </summary>
    public class DetalleVentaRequestDto
    {
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
    }
}
