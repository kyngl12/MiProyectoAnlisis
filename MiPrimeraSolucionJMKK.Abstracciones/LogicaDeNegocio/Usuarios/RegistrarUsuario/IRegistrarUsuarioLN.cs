using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario
{
    public interface IRegistrarUsuarioLN
    {
        bool Registrar(UsuarioDto elUsuario);
    }
}