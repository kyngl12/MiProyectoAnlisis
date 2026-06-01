using System;
using System.Collections.Generic;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.ObtenerTodosLosUsuarios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.ObtenerTodosLosUsuarios
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
                var usuarios = _elContexto.Usuarios.ToList();
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
            {
                usuariosDto.Add(ConvertirADto(usuario));
            }

            return usuariosDto;
        }

        private UsuarioDto ConvertirADto(UsuariosEntidad usuario)
        {
            return new UsuarioDto
            {
                IdUsuario = usuario.IdUsuario,
                IdComercio = usuario.IdComercio,
                IdNetUser = usuario.IdNetUser,
                Nombres = usuario.Nombres,
                PrimerApellido = usuario.PrimerApellido,
                SegundoApellido = usuario.SegundoApellido,
                Identificacion = usuario.Identificacion,
                CorreoElectronico = usuario.CorreoElectronico,
                FechaDeRegistro = usuario.FechaDeRegistro,
                FechaDeModificacion = usuario.FechaDeModificacion,
                Estado = usuario.Estado
            };
        }
    }
}
