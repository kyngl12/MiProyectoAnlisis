using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas
{
    /// <summary>
    /// GPV-001: logica de negocio para registrar una venta con su detalle.
    /// </summary>
    public interface IRegistrarVentaLN
    {
        VentaOperacionResultDto<int> Registrar(VentaRequestDto venta);
    }
}
