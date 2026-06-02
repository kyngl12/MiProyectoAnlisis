using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.InactivarUsuario
{
    public interface IInactivarUsuarioAD
    {
        int Inactivar(string cedula);
    }
}
