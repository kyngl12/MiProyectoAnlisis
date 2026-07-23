using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas
{
    /// <summary>
    /// GPV-002 / GPV-006: logica de negocio para procesar el pago de
    /// una venta y generar su comprobante.
    /// </summary>
    public interface IProcesarVentaLN
    {
        VentaOperacionResultDto<ComprobanteVentaDto> Procesar(int idVenta, ProcesarVentaRequestDto request);
    }
}
