using System;
using System.Collections.Generic;
using GestionPubRock.AccesoADatos.Reservaciones;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reservacion;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Reservaciones
{
    public class ObtenerReservacionesLN
    {
        private readonly ObtenerReservacionesAD _ad;

        public ObtenerReservacionesLN()
        {
            _ad = new ObtenerReservacionesAD();
        }

        public List<ReservacionDto> Obtener(DateTime? fecha, bool? estado)
        {
            try
            {
                return _ad.Obtener(fecha, estado);
            }
            catch
            {
                throw;
            }
        }
    }
}
