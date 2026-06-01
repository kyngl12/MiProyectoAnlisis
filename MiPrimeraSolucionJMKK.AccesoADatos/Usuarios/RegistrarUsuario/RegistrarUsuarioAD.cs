using System;
using System.Collections.Generic;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.RegistrarUsuario
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
