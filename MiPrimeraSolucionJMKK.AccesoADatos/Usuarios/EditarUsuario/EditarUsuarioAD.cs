using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.EditarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using System.Linq;

namespace GestionPubRock.AccesoADatos.Usuarios.EditarUsuario
{
    public class EditarUsuarioAD : IEditarUsuarioAD
    {
        private Contexto _elContexto;

        public EditarUsuarioAD()
        {
            _elContexto = new Contexto();
        }

        public int Editar(UsuarioDto usuario)
        {
            try
            {
                var usuarioExistente = _elContexto.Usuarios
                    .FirstOrDefault(u => u.Cedula == usuario.Cedula);

                if (usuarioExistente == null)
                    return -1;

                // Verificar que el correo o teléfono no estén en uso por otro usuario
                bool correoDuplicado = _elContexto.Usuarios
                    .Any(u => u.Correo == usuario.CorreoElectronico && u.Cedula != usuarioExistente.Cedula);
                if (correoDuplicado) return -2;

                bool telefonoDuplicado = _elContexto.Usuarios
                    .Any(u => u.Telefono == usuario.Telefono && u.Cedula != usuarioExistente.Cedula);
                if (telefonoDuplicado) return -3;

 
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.ApellidoPaterno = usuario.ApellidoPaterno;
                usuarioExistente.ApellidoMaterno = usuario.ApellidoMaterno;
                usuarioExistente.IdTipoUsuario = usuario.IdTipoUsuario;
                usuarioExistente.IdEstado = usuario.IdEstado;
                    usuarioExistente.Correo = usuario.CorreoElectronico; 
                usuarioExistente.Telefono = usuario.Telefono;

                return _elContexto.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}