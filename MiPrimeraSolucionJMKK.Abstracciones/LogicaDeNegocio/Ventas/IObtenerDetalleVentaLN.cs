using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas
{
    /// <summary>
    /// GPV-005: logica de negocio para obtener el detalle de una venta.
    /// </summary>
    public interface IObtenerDetalleVentaLN
    {
        VentaOperacionResultDto<VentaResponseDto> ObtenerDetalle(int idVenta);
    }
}
