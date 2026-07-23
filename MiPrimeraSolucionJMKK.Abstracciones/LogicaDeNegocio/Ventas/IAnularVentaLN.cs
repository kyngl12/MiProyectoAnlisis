using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas
{
    /// <summary>
    /// GPV-003 / GPV-006: logica de negocio para anular una venta.
    /// </summary>
    public interface IAnularVentaLN
    {
        VentaOperacionResultDto<bool> Anular(int idVenta, AnularVentaRequestDto request);
    }
}
