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
                // Validar duplicados por cedula, correo o telefono de forma individual
                var existeCedula = _elContexto.Database.SqlQuery<int?>("SELECT 1 FROM PUBROCK_USUARIO_TB WHERE CEDULA = @p0", usuario.Cedula).FirstOrDefault();
                if (existeCedula != null) return -1; // cedula duplicada

                var existeCorreo = _elContexto.Database.SqlQuery<int?>("SELECT 1 FROM PUBROCK_USUARIO_TB WHERE CORREO = @p0", usuario.CorreoElectronico).FirstOrDefault();
                if (existeCorreo != null) return -2; // correo duplicado

                var existeTelefono = _elContexto.Database.SqlQuery<int?>("SELECT 1 FROM PUBROCK_USUARIO_TB WHERE TELEFONO = @p0", usuario.Telefono).FirstOrDefault();
                if (existeTelefono != null) return -3; // telefono duplicado

                UsuariosEntidad usuarioAGuardar = ConvertirAEntidad(usuario);
                _elContexto.Usuarios.Add(usuarioAGuardar);
                int cantidadDeRegistrosAlmacenados = _elContexto.SaveChanges();

                return cantidadDeRegistrosAlmacenados;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                // Error de base de datos inesperado
                return -99;
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