using System;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad
{
    public interface IGenerarBalanceAD
    {
        BalanceDto Generar(DateTime fechaInicio, DateTime fechaFin);

        bool ExistenDatosEnRango(DateTime fechaInicio, DateTime fechaFin);
    }
}
