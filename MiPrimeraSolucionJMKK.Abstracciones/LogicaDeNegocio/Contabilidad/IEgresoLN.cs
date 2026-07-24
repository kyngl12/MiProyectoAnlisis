using System;
using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad
{
    public interface IRegistrarEgresoLN
    {
        bool Registrar(EgresoDto egreso);
    }

    public interface IObtenerEgresosLN
    {
        List<EgresoDto> Obtener(DateTime? fechaInicio, DateTime? fechaFin);
    }
}
