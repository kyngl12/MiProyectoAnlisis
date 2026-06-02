using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.InactivarUsuario
{
    public interface IInactivarUsuarioLN
    {
        int Inactivar(string cedula);
    }
}
