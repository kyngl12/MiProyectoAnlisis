using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.EditarUsuario
{
    public interface IEditarUsuarioLN
    {
        bool Editar(UsuarioDto elUsuario);
    }
}