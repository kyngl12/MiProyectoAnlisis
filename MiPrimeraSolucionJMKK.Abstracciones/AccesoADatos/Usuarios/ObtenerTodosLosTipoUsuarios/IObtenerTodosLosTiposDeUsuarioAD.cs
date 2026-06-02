using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.ObtenerTodosLosTipoUsuarios
{
    public interface IObtenerTodosLosTiposDeUsuarioAD
    {
        List<TipoUsuarioDto> ObtenerTodos();
    }
}
