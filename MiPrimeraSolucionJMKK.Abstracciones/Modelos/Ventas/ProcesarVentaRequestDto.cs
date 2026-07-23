namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas
{
    /// <summary>
    /// GPV-002: datos necesarios para procesar el pago de una venta.
    /// </summary>
    public class ProcesarVentaRequestDto
    {
        public int IdTipoPago { get; set; }
    }
}
