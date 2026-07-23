using System;
using System.Collections.Generic;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas
{
    /// <summary>
    /// GPV-005: informacion general y detalle completo de una venta.
    /// </summary>
    public class VentaResponseDto
    {
        public int IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public int? IdCliente { get; set; }
        public int IdEmpleado { get; set; }
        public string Estado { get; set; }
        public decimal SubtotalGeneral { get; set; }
        public decimal TotalImpuestos { get; set; }
        public decimal Total { get; set; }
        public DateTime? FechaAnulacion { get; set; }
        public string MotivoAnulacion { get; set; }
        public List<DetalleVentaResponseDto> Detalle { get; set; } = new List<DetalleVentaResponseDto>();
    }
}
