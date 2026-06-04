using System;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ordenes
{
    public class OrdenDto
    {
        public int? NumeroOrden { get; set; }
        public int IdProducto { get; set; }
        public int Cantidad { get; set; }
        public int IdProveedor { get; set; }
        public DateTime FechaEntregaEstim { get; set; }
        public string Observaciones { get; set; }
        public string Estado { get; set; }
    }
}
