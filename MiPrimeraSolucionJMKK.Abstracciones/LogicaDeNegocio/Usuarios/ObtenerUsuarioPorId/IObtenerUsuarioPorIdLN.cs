using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId
{
    public interface IObtenerUsuarioPorIdLN
    {
        UsuarioDto Obtener(int idUsuario);
    }
}