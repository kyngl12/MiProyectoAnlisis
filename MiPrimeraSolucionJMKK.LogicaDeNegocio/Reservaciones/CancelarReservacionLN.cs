using System;
using GestionPubRock.AccesoADatos.Reservaciones;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Reservaciones
{
    public class CancelarReservacionLN
    {
        private readonly CancelarReservacionAD _ad;

        public CancelarReservacionLN()
        {
            _ad = new CancelarReservacionAD();
        }

        public void Cancelar(int id)
        {
            var result = _ad.Cancelar(id);
            switch (result)
            {
                case -2:
                    throw new InvalidOperationException("La reservación indicada no existe.");
                case -3:
                    throw new InvalidOperationException("La reservación ya se encuentra cancelada.");
                case -99:
                    throw new InvalidOperationException("Error al acceder a la base de datos.");
                default:
                    return;
            }
        }
    }
}
