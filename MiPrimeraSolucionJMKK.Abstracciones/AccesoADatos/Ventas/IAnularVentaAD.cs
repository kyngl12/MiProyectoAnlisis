namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas
{
    /// <summary>
    /// GPV-003 / GPV-006: acceso a datos para anular una venta
    /// (borrado logico + restauracion de inventario si aplica).
    /// </summary>
    public interface IAnularVentaAD
    {
        /// <summary>
        /// Invoca SP_PUBROCK_ANULAR_VENTA. Retorna el codigo de
        /// resultado del procedimiento almacenado.
        /// </summary>
        int Anular(int idVenta, string motivoAnulacion);
    }
}
