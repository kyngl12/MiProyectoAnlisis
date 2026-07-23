using System.Collections.Generic;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    public class ObtenerImpuestosAD : IObtenerImpuestosAD
    {
        private readonly Contexto _elContexto;

        public ObtenerImpuestosAD()
        {
            _elContexto = new Contexto();
        }

        public List<ImpuestoDto> Obtener()
        {
            return _elContexto.Impuestos
                .Where(i => i.IdEstado == 1)
                .OrderBy(i => i.NombreImpuesto)
                .ToList()
                .Select(i => new ImpuestoDto
                {
                    IdImpuesto = i.IdImpuesto,
                    NombreImpuesto = i.NombreImpuesto,
                    Porcentaje = i.Porcentaje
                })
                .ToList();
        }
    }
}
