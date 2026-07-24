using System;

namespace MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora
{
    public class BitacoraContableFiltroDto
    {
        public string Cedula { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}
