using System;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reservacion
{
    public class ReservacionDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public bool Estado { get; set; }
        public string Cliente { get; set; }
        public string Observaciones { get; set; }
    }
}
