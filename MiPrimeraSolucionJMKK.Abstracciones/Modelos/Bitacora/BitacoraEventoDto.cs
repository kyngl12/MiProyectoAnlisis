using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora
{
    public class BitacoraEventoDto
    {
        public int IdEvento { get; set; }
        public string TablaDeEvento { get; set; }
        public string TipoDeEvento { get; set; }
        public DateTime FechaDeEvento { get; set; }
        public string DescripcionDeEvento { get; set; }
        public string StackTrace { get; set; }
        public string DatosAnteriores { get; set; }
        public string DatosPosteriores { get; set; }
    }
}
