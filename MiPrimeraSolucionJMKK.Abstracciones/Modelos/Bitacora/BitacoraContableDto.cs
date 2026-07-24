using System;

namespace MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora
{
    public class BitacoraContableDto
    {
        public int IdBitacora { get; set; }
        public DateTime FechaHora { get; set; }
        public string AccionRealizada { get; set; }
        public string Descripcion { get; set; }
        public string Cedula { get; set; }
        public string NombreUsuario { get; set; }
        public string Modulo { get; set; }
    }
}
