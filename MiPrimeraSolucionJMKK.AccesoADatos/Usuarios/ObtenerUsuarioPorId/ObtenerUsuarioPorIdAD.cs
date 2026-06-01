using System;
using System.Collections.Generic;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.ObtenerUsuarioPorId;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.ObtenerUsuarioPorId
{
    public class ObtenerUsuarioPorIdAD : IObtenerUsuarioPorIdAD
    {
        private Contexto _elContexto;

        public ObtenerUsuarioPorIdAD()
        {
            _elContexto = new Contexto();
        }

        public UsuarioDto ObtenerPorId(int id)
        {
            try
            {
                var usuario = _elContexto.Usuarios.FirstOrDefault(u => u.IdUsuario == id);
                if (usuario == null)
                    return null;

                return ConvertirADto(usuario);
            }
            catch
            {
                throw;
            }
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
