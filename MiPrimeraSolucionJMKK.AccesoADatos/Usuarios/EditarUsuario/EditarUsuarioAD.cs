using System;
using System.Collections.Generic;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.EditarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.EditarUsuario
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
                    return 0;

                bool cedulaDuplicada = _elContexto.Usuarios
                    .Any(u => u.Cedula == usuario.Cedula && u.Cedula != usuarioExistente.Cedula);
                if (cedulaDuplicada) return 0;

 
                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.ApellidoPaterno = usuario.ApellidoPaterno;
                usuarioExistente.ApellidoMaterno = usuario.ApellidoMaterno;
                usuarioExistente.IdTipoUsuario = usuario.IdTipoUsuario;
                usuarioExistente.IdEstado = usuario.IdEstado;

                return _elContexto.SaveChanges();
            }
            catch
            {
                throw;
            }
        }
    }
}