using System;
using System.Linq;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;

namespace GestionPubRock.AccesoADatos.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioAD : IRegistrarUsuarioAD
    {
        private Contexto _elContexto;

        public RegistrarUsuarioAD()
        {
            _elContexto = new Contexto();
        }

        public int Registrar(UsuarioDto usuario)
        {
            try
            {

                bool yaExiste = _elContexto.Usuarios.Any(u => u.Cedula == usuario.Cedula);
                if (yaExiste) return 0;

                UsuariosEntidad usuarioAGuardar = ConvertirAEntidad(usuario);
                _elContexto.Usuarios.Add(usuarioAGuardar);
                int cantidadDeRegistrosAlmacenados = _elContexto.SaveChanges();

                return cantidadDeRegistrosAlmacenados;
            }
            catch
            {
                throw;
            }
        }

        private UsuariosEntidad ConvertirAEntidad(UsuarioDto usuario)
        {
            return new UsuariosEntidad
            {
                Cedula = usuario.Cedula,
                Nombre = usuario.Nombre,
                ApellidoPaterno = usuario.ApellidoPaterno,
                ApellidoMaterno = usuario.ApellidoMaterno,
                FechaRegistro = DateTime.Now, 
                IdTipoUsuario = usuario.IdTipoUsuario,
                IdEstado = usuario.IdEstado,
                Correo = usuario.CorreoElectronico,  
                Telefono = usuario.Telefono,          
                Contrasenia = usuario.Contrasenia

            };
        }
    }
}