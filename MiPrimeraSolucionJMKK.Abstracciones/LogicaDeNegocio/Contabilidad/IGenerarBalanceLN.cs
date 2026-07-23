using System;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad
{
    public interface IGenerarBalanceLN
    {
        BalanceDto Generar(DateTime fechaInicio, DateTime fechaFin);
    }
}
