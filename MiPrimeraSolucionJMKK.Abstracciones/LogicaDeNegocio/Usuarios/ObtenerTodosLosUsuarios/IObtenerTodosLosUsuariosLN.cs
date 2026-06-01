using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using System.Collections.Generic;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios
{
    public interface IObtenerTodosLosUsuariosLN
    {
        List<UsuarioDto> Obtener();
    }
}