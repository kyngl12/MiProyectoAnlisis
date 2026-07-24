using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    public class ActualizarImpuestoAD : IActualizarImpuestoAD
    {
        private readonly Contexto _elContexto;

        public ActualizarImpuestoAD()
        {
            _elContexto = new Contexto();
        }

        public bool Actualizar(ImpuestoDto impuesto)
        {
            try
            {
                var entidad = _elContexto.Impuestos
                    .FirstOrDefault(i => i.IdImpuesto == impuesto.IdImpuesto);

                if (entidad == null)
                    return false;

                // Las ventas ya facturadas (PUBROCK_VENTA_IMPUESTO_TB) guardan
                // el monto de impuesto ya calculado, por lo que actualizar la
                // tasa aqui solo afecta los calculos futuros, tal como pide
                // el Escenario 2.
                entidad.Porcentaje = impuesto.Porcentaje;

                return _elContexto.SaveChanges() > 0;
            }
            catch
            {
                throw;
            }
        }
    }
}
