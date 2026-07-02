using System;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reservacion;
using GestionPubRock.AccesoADatos.Reservaciones;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Reservaciones
{
    public class RegistrarReservacionLN
    {
        private readonly RegistrarReservacionAD _ad;

        public RegistrarReservacionLN()
        {
            _ad = new RegistrarReservacionAD();
        }

        public void Registrar(ReservacionRequestDto dto)
        {
            // Validaciones de negocio
            if (dto.Fecha.Date < DateTime.Now.Date)
                throw new ArgumentException("La fecha de reservación no puede ser anterior a la fecha actual.");

            if (dto.HoraFin <= dto.HoraInicio)
                throw new ArgumentException("La hora de fin debe ser mayor a la hora de inicio.");

            if (dto.CantidadPersonas <= 0)
                throw new ArgumentException("La cantidad de personas debe ser mayor a cero.");

            // Llamar AD
            var result = _ad.Registrar(dto);

            switch (result)
            {
                case -2:
                    throw new InvalidOperationException("El cliente indicado no existe.");
                case -3:
                    throw new InvalidOperationException("La mesa indicada no existe.");
                case -4:
                    throw new InvalidOperationException("La cantidad de personas excede la capacidad de la mesa.");
                case -5:
                    throw new InvalidOperationException("La mesa no está disponible en el horario solicitado.");
                case -6:
                    throw new InvalidOperationException("El cliente ya tiene una reservación en el mismo horario.");
                case -99:
                    throw new InvalidOperationException("Error al acceder a la base de datos.");
                default:
                    // éxito (result == 1 normalmente)
                    return;
            }
        }
    }
}
