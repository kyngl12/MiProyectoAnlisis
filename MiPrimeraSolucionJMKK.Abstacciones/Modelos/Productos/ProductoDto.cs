namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Productos
{
    public class ProductoDto
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
