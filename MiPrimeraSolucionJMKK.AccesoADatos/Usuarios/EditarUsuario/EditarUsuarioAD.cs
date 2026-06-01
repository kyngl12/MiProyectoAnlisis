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
                var usuarioExistente = _elContexto.Usuarios.FirstOrDefault(u => u.IdUsuario == usuario.IdUsuario);
                
                if (usuarioExistente == null)
                    return 0;

                usuarioExistente.Nombres = usuario.Nombres;
                usuarioExistente.PrimerApellido = usuario.PrimerApellido;
                usuarioExistente.SegundoApellido = usuario.SegundoApellido;
                usuarioExistente.Identificacion = usuario.Identificacion;
                usuarioExistente.CorreoElectronico = usuario.CorreoElectronico;
                usuarioExistente.Estado = usuario.Estado;
                usuarioExistente.FechaDeModificacion = usuario.FechaDeModificacion;

                int cantidadDeRegistrosActualizados = _elContexto.SaveChanges();

                return cantidadDeRegistrosActualizados;
            }
            catch
            {
                throw;
            }
        }
    }
}
