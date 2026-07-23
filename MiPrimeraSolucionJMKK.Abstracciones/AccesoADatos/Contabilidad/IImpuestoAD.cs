using System.Collections.Generic;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad
{
    public interface IObtenerImpuestosAD
    {
        List<ImpuestoDto> Obtener();
    }

    public interface IActualizarImpuestoAD
    {
        bool Actualizar(ImpuestoDto impuesto);
    }
}
