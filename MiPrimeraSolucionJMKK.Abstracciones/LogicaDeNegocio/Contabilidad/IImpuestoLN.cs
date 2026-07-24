using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad
{
    public interface IObtenerImpuestosLN
    {
        List<ImpuestoDto> Obtener();
    }

    public interface IActualizarImpuestoLN
    {
        bool Actualizar(ImpuestoDto impuesto);
    }

    /// <summary>
    /// Calcula el desglose de IVA y cargo por servicio para el
    /// subtotal de una orden, usando las tasas configuradas
    /// actualmente (Escenario 1 y 3 de CON-005).
    /// </summary>
    public interface ICalcularImpuestosLN
    {
        CalculoImpuestoDto Calcular(decimal subtotal, bool exento);
    }
}
