using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas
{
    /// <summary>
    /// GPV-005: acceso a datos para obtener el detalle completo de una venta.
    /// </summary>
    public interface IObtenerDetalleVentaAD
    {
        /// <summary>
        /// Invoca SP_PUBROCK_OBTENER_DETALLE_VENTA. Retorna null si la venta no existe.
        /// </summary>
        VentaResponseDto ObtenerDetalle(int idVenta);
    }
}
