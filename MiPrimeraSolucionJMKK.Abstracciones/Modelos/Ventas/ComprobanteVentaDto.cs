using System;
using System.Collections.Generic;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas
{
    /// <summary>
    /// GPV-002: comprobante generado luego de procesar el pago de una venta.
    /// </summary>
    public class ComprobanteVentaDto
    {
        public int IdFactura { get; set; }
        public string NumeroFactura { get; set; }
        public int IdVenta { get; set; }
        public DateTime FechaFactura { get; set; }
        public int? IdCliente { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
        public List<DetalleVentaResponseDto> Detalle { get; set; } = new List<DetalleVentaResponseDto>();
    }
}
