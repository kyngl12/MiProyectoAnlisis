using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;
using GestionPubRock.AccesoADatos.Ventas;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Ventas
{
    /// <summary>
    /// GPV-003 / GPV-006: valida y anula una venta mediante borrado
    /// logico de estado, restaurando el inventario si la venta ya
    /// habia sido pagada.
    /// </summary>
    public class AnularVentaLN : IAnularVentaLN
    {
        private readonly IAnularVentaAD _ad;

        public AnularVentaLN() : this(new AnularVentaAD())
        {
        }

        public AnularVentaLN(IAnularVentaAD ad)
        {
            _ad = ad;
        }

        public VentaOperacionResultDto<bool> Anular(int idVenta, AnularVentaRequestDto request)
        {
            if (idVenta <= 0)
            {
                return VentaOperacionResultDto<bool>.Error(VentaResultadoCodigo.DatosInvalidos, "El identificador de la venta no es valido.");
            }

            var motivo = request != null ? request.MotivoAnulacion : null;
            int resultadoSp = _ad.Anular(idVenta, motivo);

            switch (resultadoSp)
            {
                case 1:
                    return VentaOperacionResultDto<bool>.Ok(true);
                case -1:
                    return VentaOperacionResultDto<bool>.Error(VentaResultadoCodigo.VentaNoExiste, "La venta indicada no existe.");
                case -2:
                    return VentaOperacionResultDto<bool>.Error(VentaResultadoCodigo.VentaYaAnulada, "La venta ya se encuentra anulada.");
                default:
                    return VentaOperacionResultDto<bool>.Error(VentaResultadoCodigo.ErrorInterno, "No se pudo anular la venta. Intente nuevamente.");
            }
        }
    }
}
