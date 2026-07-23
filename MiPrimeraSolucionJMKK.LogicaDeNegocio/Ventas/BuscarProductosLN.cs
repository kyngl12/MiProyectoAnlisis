using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;
using GestionPubRock.AccesoADatos.Ventas;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Ventas
{
    /// <summary>
    /// GPV-004: busca productos activos con stock disponible para
    /// agregarlos a una venta.
    /// </summary>
    public class BuscarProductosLN : IBuscarProductosLN
    {
        private readonly IBuscarProductosAD _ad;

        public BuscarProductosLN() : this(new BuscarProductosAD())
        {
        }

        public BuscarProductosLN(IBuscarProductosAD ad)
        {
            _ad = ad;
        }

        public VentaOperacionResultDto<List<BuscarProductoResponseDto>> Buscar(string textoBusqueda)
        {
            var resultado = _ad.Buscar(textoBusqueda);
            return VentaOperacionResultDto<List<BuscarProductoResponseDto>>.Ok(resultado);
        }
    }
}
