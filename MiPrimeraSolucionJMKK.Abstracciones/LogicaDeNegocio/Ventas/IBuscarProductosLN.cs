using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas
{
    /// <summary>
    /// GPV-004: logica de negocio para buscar productos disponibles para la venta.
    /// </summary>
    public interface IBuscarProductosLN
    {
        VentaOperacionResultDto<List<BuscarProductoResponseDto>> Buscar(string textoBusqueda);
    }
}
