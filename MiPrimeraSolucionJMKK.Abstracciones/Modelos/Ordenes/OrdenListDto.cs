using System;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ordenes
{
    public class OrdenListDto
    {
        public int IdOrden { get; set; }
        public DateTime FechaOrden { get; set; }
        public string Observaciones { get; set; }
        public int CantidadItems { get; set; }
        public int? IdEstadoOrden { get; set; }
        public string EstadoOrdenDescripcion { get; set; }
    }
}
