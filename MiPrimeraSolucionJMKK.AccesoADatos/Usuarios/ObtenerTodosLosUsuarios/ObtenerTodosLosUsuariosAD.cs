using System.Collections.Generic;
using System.Linq;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.ObtenerTodosLosUsuarios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;

namespace GestionPubRock.AccesoADatos.Usuarios.ObtenerTodosLosUsuarios
{
    public class ObtenerTodosLosUsuariosAD : IObtenerTodosLosUsuariosAD
    {
        private Contexto _elContexto;

        public ObtenerTodosLosUsuariosAD()
        {
            _elContexto = new Contexto();
        }

        public List<UsuarioDto> ObtenerTodos()
        {
            try
            {
                var usuarios = _elContexto.Usuarios
                    .Include("TipoUsuario") 
                    .ToList();
                return ConvertirAListaDto(usuarios);
            }
            catch
            {
                throw;
            }
        }

        private List<UsuarioDto> ConvertirAListaDto(List<UsuariosEntidad> usuariosEntidad)
        {
            List<UsuarioDto> usuariosDto = new List<UsuarioDto>();
            foreach (var usuario in usuariosEntidad)
                usuariosDto.Add(ConvertirADto(usuario));
            return usuariosDto;
        }

        private UsuarioDto ConvertirADto(UsuariosEntidad usuario)
        {
            return new UsuarioDto
            {
                Cedula = usuario.Cedula,
                Nombre = usuario.Nombre,
                ApellidoPaterno = usuario.ApellidoPaterno,
                ApellidoMaterno = usuario.ApellidoMaterno,
                FechaRegistro = usuario.FechaRegistro,
                IdTipoUsuario = usuario.IdTipoUsuario,
                IdEstado = usuario.IdEstado,
                DescripcionTipoUsuario = usuario.TipoUsuario != null
                    ? usuario.TipoUsuario.Descripcion : "Sin rol",
                CorreoElectronico = usuario.Correo,  
                Telefono = usuario.Telefono
            };
        }
    }
}
