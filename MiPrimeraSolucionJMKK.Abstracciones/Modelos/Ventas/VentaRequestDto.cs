using System.Collections.Generic;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas
{
    /// <summary>
    /// GPV-001: datos necesarios para registrar una venta nueva.
    /// </summary>
    public class VentaRequestDto
    {
        public int IdEmpleado { get; set; }
        public int? IdCliente { get; set; }
        public List<DetalleVentaRequestDto> Detalle { get; set; } = new List<DetalleVentaRequestDto>();
    }
}
