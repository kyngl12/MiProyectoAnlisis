using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios
{
    public class TipoUsuarioDto
    {
        public int IdTipoUsuario { get; set; }
        public string Descripcion { get; set; }
        public int IdEstado { get; set; }
    }
}