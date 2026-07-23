using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;
using GestionPubRock.AccesoADatos.Ventas;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Ventas
{
    /// <summary>
    /// GPV-001: valida y registra una venta con su detalle. No
    /// descuenta inventario (eso ocurre al procesar el pago, GPV-002).
    /// </summary>
    public class RegistrarVentaLN : IRegistrarVentaLN
    {
        private readonly IRegistrarVentaAD _ad;

        public RegistrarVentaLN() : this(new RegistrarVentaAD())
        {
        }

        public RegistrarVentaLN(IRegistrarVentaAD ad)
        {
            _ad = ad;
        }

        public VentaOperacionResultDto<int> Registrar(VentaRequestDto venta)
        {
            if (GestionPubRock.AccesoADatos.Contabilidad.CierreCajaGuard.EstaLaCajaCerradaParaLaFecha(System.DateTime.Now))
            {
                return VentaOperacionResultDto<int>.Error(VentaResultadoCodigo.DatosInvalidos, "La caja ya se encuentra cerrada para esta jornada. No es posible registrar nuevas ventas.");
            }

            if (venta == null || venta.IdEmpleado <= 0)
            {
                return VentaOperacionResultDto<int>.Error(VentaResultadoCodigo.DatosInvalidos, "Debe indicar el empleado que registra la venta.");
            }

            if (venta.Detalle == null || venta.Detalle.Count == 0)
            {
                return VentaOperacionResultDto<int>.Error(VentaResultadoCodigo.SinProductos, "La venta debe contener al menos un producto.");
            }

            foreach (var linea in venta.Detalle)
            {
                if (linea.IdProducto <= 0)
                {
                    return VentaOperacionResultDto<int>.Error(VentaResultadoCodigo.DatosInvalidos, "Debe indicar un producto valido en cada linea de la venta.");
                }

                if (linea.Cantidad <= 0)
                {
                    return VentaOperacionResultDto<int>.Error(VentaResultadoCodigo.CantidadInvalida, "La cantidad de cada producto debe ser mayor que cero.");
                }
            }

            int idVentaCreada;
            int resultadoSp = _ad.Registrar(venta, out idVentaCreada);

            switch (resultadoSp)
            {
                case 1:
                    return VentaOperacionResultDto<int>.Ok(idVentaCreada);
                case -1:
                    return VentaOperacionResultDto<int>.Error(VentaResultadoCodigo.ProductoNoExiste, "Uno o mas productos no existen o estan inactivos.");
                case -2:
                    return VentaOperacionResultDto<int>.Error(VentaResultadoCodigo.CantidadInvalida, "La cantidad de cada producto debe ser mayor que cero.");
                case -3:
                    return VentaOperacionResultDto<int>.Error(VentaResultadoCodigo.StockInsuficiente, "No hay stock suficiente para uno o mas productos.");
                case -4:
                    return VentaOperacionResultDto<int>.Error(VentaResultadoCodigo.SinProductos, "La venta debe contener al menos un producto.");
                default:
                    return VentaOperacionResultDto<int>.Error(VentaResultadoCodigo.ErrorInterno, "No se pudo registrar la venta. Intente nuevamente.");
            }
        }
    }
}
