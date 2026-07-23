using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas
{
    /// <summary>
    /// GPV-002 / GPV-006: acceso a datos para procesar el pago de una
    /// venta (descuenta inventario y genera el comprobante).
    /// </summary>
    public interface IProcesarVentaAD
    {
        /// <summary>
        /// Invoca SP_PUBROCK_PROCESAR_VENTA. Retorna el codigo de
        /// resultado del procedimiento y, cuando es exitoso, el
        /// comprobante generado con su detalle.
        /// </summary>
        int Procesar(int idVenta, int idTipoPago, out ComprobanteVentaDto comprobante);
    }
}
