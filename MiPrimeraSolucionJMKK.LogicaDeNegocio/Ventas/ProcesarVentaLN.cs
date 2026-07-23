using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;
using GestionPubRock.AccesoADatos.Ventas;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Ventas
{
    /// <summary>
    /// GPV-002 / GPV-006: valida y procesa el pago de una venta,
    /// descontando inventario y generando el comprobante en una unica
    /// transaccion (a nivel de Stored Procedure).
    /// </summary>
    public class ProcesarVentaLN : IProcesarVentaLN
    {
        private readonly IProcesarVentaAD _ad;

        public ProcesarVentaLN() : this(new ProcesarVentaAD())
        {
        }

        public ProcesarVentaLN(IProcesarVentaAD ad)
        {
            _ad = ad;
        }

        public VentaOperacionResultDto<ComprobanteVentaDto> Procesar(int idVenta, ProcesarVentaRequestDto request)
        {
            if (idVenta <= 0)
            {
                return VentaOperacionResultDto<ComprobanteVentaDto>.Error(VentaResultadoCodigo.DatosInvalidos, "El identificador de la venta no es valido.");
            }

            if (request == null || request.IdTipoPago <= 0)
            {
                return VentaOperacionResultDto<ComprobanteVentaDto>.Error(VentaResultadoCodigo.DatosInvalidos, "Debe indicar el metodo de pago.");
            }

            ComprobanteVentaDto comprobante;
            int resultadoSp = _ad.Procesar(idVenta, request.IdTipoPago, out comprobante);

            switch (resultadoSp)
            {
                case 1:
                    return VentaOperacionResultDto<ComprobanteVentaDto>.Ok(comprobante);
                case -1:
                    return VentaOperacionResultDto<ComprobanteVentaDto>.Error(VentaResultadoCodigo.VentaNoExiste, "La venta indicada no existe.");
                case -2:
                    return VentaOperacionResultDto<ComprobanteVentaDto>.Error(VentaResultadoCodigo.VentaYaPagada, "La venta ya fue procesada anteriormente.");
                case -3:
                    return VentaOperacionResultDto<ComprobanteVentaDto>.Error(VentaResultadoCodigo.VentaAnulada, "No se puede procesar una venta anulada.");
                case -4:
                    return VentaOperacionResultDto<ComprobanteVentaDto>.Error(VentaResultadoCodigo.SinProductos, "La venta no tiene productos asociados.");
                case -5:
                    return VentaOperacionResultDto<ComprobanteVentaDto>.Error(VentaResultadoCodigo.StockInsuficiente, "No hay stock suficiente para completar el pago.");
                default:
                    return VentaOperacionResultDto<ComprobanteVentaDto>.Error(VentaResultadoCodigo.ErrorInterno, "No se pudo procesar la venta. Intente nuevamente.");
            }
        }
    }
}
