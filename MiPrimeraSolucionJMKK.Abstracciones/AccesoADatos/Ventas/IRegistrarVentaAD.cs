using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas
{
    /// <summary>
    /// GPV-001: acceso a datos para registrar una venta y su detalle.
    /// </summary>
    public interface IRegistrarVentaAD
    {
        /// <summary>
        /// Invoca SP_PUBROCK_REGISTRAR_VENTA. Retorna el Id de la venta
        /// creada y el codigo de resultado del procedimiento almacenado
        /// (1 = OK, negativo = error de validacion).
        /// </summary>
        int Registrar(VentaRequestDto venta, out int idVentaCreada);
    }
}
