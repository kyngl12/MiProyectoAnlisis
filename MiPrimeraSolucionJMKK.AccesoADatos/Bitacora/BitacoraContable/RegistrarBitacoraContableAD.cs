using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstacciones.AccesoADatos.Bitacora.BitacoraContable;
using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;

namespace GestionPubRock.AccesoADatos.Bitacora.BitacoraContable
{
    public class RegistrarBitacoraContableAD : IRegistrarBitacoraContableAD
    {
        private readonly Contexto _elContexto;

        public RegistrarBitacoraContableAD()
        {
            _elContexto = new Contexto();
        }

        public void Registrar(BitacoraContableDto evento)
        {
            try
            {
                var entidad = new BitacoraContableEntidad
                {
                    FechaHora = evento.FechaHora,
                    AccionRealizada = evento.AccionRealizada,
                    Descripcion = evento.Descripcion,
                    Cedula = string.IsNullOrWhiteSpace(evento.Cedula) ? null : evento.Cedula,
                    Modulo = evento.Modulo,
                    IdEstado = 1
                };

                _elContexto.BitacoraContable.Add(entidad);
                _elContexto.SaveChanges();
            }
            catch
            {
                // La bitacora nunca debe interrumpir la operacion de negocio
                // que la origino.
            }
        }
    }
}
