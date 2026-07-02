using System;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reservacion
{
    public class ReservacionRequestDto
    {
        public DateTime Fecha { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public int CantidadPersonas { get; set; }
        public int IdCliente { get; set; }
        public int IdMesa { get; set; }
        public string Observaciones { get; set; }
    }
}
