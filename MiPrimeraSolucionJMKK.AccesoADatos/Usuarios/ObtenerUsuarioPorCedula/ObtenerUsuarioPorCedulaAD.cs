using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.ObtenerUsuarioPorCedula;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.ObtenerUsuarioPorId
{
    public class ObtenerUsuarioPorCedulaAD : IObtenerUsuarioPorCedulaAD
    {
        private Contexto _elContexto;

        public ObtenerUsuarioPorCedulaAD()
        {
            _elContexto = new Contexto();
        }

        public UsuarioDto ObtenerPorCedula(string cedula)
        {
            try
            {
                var usuario = _elContexto.Usuarios
                    .Include("TipoUsuario")
                    .FirstOrDefault(u => u.Cedula == cedula);

                if (usuario == null) return null;

                return ConvertirADto(usuario);
            }
            catch { throw; }
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
                    ? usuario.TipoUsuario.Descripcion : "Sin rol"
            };
        }
    }
}
