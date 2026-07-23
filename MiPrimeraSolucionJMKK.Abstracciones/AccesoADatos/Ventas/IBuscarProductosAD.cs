using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas
{
    /// <summary>
    /// GPV-004: acceso a datos para buscar productos activos con stock disponible.
    /// </summary>
    public interface IBuscarProductosAD
    {
        /// <summary>
        /// Invoca SP_PUBROCK_BUSCAR_PRODUCTOS con busqueda parcial por
        /// nombre, codigo de barras o categoria.
        /// </summary>
        List<BuscarProductoResponseDto> Buscar(string textoBusqueda);
    }
}
