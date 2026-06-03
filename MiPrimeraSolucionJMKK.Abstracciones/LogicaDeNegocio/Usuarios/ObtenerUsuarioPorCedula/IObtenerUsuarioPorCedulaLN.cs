using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorCedula
{
    public interface IObtenerUsuarioPorCedulaLN
    {
        UsuarioDto Obtener(string cedula);
    }
}