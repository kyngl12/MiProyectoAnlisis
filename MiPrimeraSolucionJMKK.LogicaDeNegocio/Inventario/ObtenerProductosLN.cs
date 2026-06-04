using System.Collections.Generic;
using GestionPubRock.AccesoADatos.Inventario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Productos;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Inventario
{
    public class ObtenerProductosLN
    {
        private readonly ObtenerProductosAD _ad;

        public ObtenerProductosLN()
        {
            _ad = new ObtenerProductosAD();
        }

        public List<ProductoDto> ObtenerTodos()
        {
            return _ad.ObtenerTodos();
        }

        public List<ProductoDto> Buscar(string termino)
        {
            return _ad.Buscar(termino);
        }

        public List<ProductoDto> ObtenerPorFiltro(string categoria, decimal? minCantidad, decimal? maxCantidad)
        {
            return _ad.ObtenerPorFiltro(categoria, minCantidad, maxCantidad);
        }

        public System.Collections.Generic.List<string> ObtenerCategorias()
        {
            return _ad.ObtenerCategorias();
        }
    }
}
