using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.ValidarIdentificacion
{
    public interface IValidarIdentificacionUsuarioAD
    {
        bool ExisteIdentificacion(string identificacion);
        bool ExisteIdentificacionExceptoUsuario(string identificacion, int idUsuario);
    }
}
