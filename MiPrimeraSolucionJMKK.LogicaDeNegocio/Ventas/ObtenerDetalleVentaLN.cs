using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;
using GestionPubRock.AccesoADatos.Ventas;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Ventas
{
    /// <summary>
    /// GPV-005: obtiene la informacion general y el detalle de una venta.
    /// </summary>
    public class ObtenerDetalleVentaLN : IObtenerDetalleVentaLN
    {
        private readonly IObtenerDetalleVentaAD _ad;

        public ObtenerDetalleVentaLN() : this(new ObtenerDetalleVentaAD())
        {
        }

        public ObtenerDetalleVentaLN(IObtenerDetalleVentaAD ad)
        {
            _ad = ad;
        }

        public VentaOperacionResultDto<VentaResponseDto> ObtenerDetalle(int idVenta)
        {
            if (idVenta <= 0)
            {
                return VentaOperacionResultDto<VentaResponseDto>.Error(VentaResultadoCodigo.DatosInvalidos, "El identificador de la venta no es valido.");
            }

            var venta = _ad.ObtenerDetalle(idVenta);

            if (venta == null)
            {
                return VentaOperacionResultDto<VentaResponseDto>.Error(VentaResultadoCodigo.VentaNoExiste, "La venta indicada no existe.");
            }

            return VentaOperacionResultDto<VentaResponseDto>.Ok(venta);
        }
    }
}
