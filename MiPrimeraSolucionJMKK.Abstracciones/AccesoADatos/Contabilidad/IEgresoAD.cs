using System;
using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad
{
    public interface IRegistrarEgresoAD
    {
        int Registrar(EgresoDto egreso);
    }

    public interface IObtenerEgresosAD
    {
        List<EgresoDto> Obtener(DateTime? fechaInicio, DateTime? fechaFin);
    }
}
