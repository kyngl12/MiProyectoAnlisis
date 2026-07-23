namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad
{
    public class CalculoImpuestoDto
    {
        public decimal Subtotal { get; set; }
        public bool Exento { get; set; }

        public decimal PorcentajeIva { get; set; }
        public decimal PorcentajeServicio { get; set; }

        public decimal MontoIva { get; set; }
        public decimal MontoServicio { get; set; }
        public decimal Total { get; set; }
    }
}
