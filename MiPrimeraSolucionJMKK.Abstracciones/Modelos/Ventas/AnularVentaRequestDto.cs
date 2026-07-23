namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas
{
    /// <summary>
    /// GPV-003: motivo opcional de anulacion de una venta.
    /// </summary>
    public class AnularVentaRequestDto
    {
        public string MotivoAnulacion { get; set; }
    }
}
