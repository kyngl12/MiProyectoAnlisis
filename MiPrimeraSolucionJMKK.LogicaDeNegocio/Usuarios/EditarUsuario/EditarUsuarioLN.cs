using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.EditarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.EditarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.AccesoADatos.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.EditarUsuario;
using MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.ValidarIdentificacion;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.EditarUsuario
{
    public class EditarUsuarioLN : IEditarUsuarioLN
    {
        private IEditarUsuarioAD _editarUsuarioAD;
        private ValidarIdentificacionUsuarioAD _validarIdentificacionUsuarioAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public EditarUsuarioLN()
        {
            _editarUsuarioAD = new EditarUsuarioAD();
            _validarIdentificacionUsuarioAD = new ValidarIdentificacionUsuarioAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public bool Editar(UsuarioDto usuario)
        {
            try
            {
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(usuario.Nombres)) return false;
                if (string.IsNullOrWhiteSpace(usuario.PrimerApellido)) return false;
                if (string.IsNullOrWhiteSpace(usuario.SegundoApellido)) return false;
                if (string.IsNullOrWhiteSpace(usuario.Identificacion)) return false;
                if (string.IsNullOrWhiteSpace(usuario.CorreoElectronico)) return false;

                // Validar que la identificación no exista para otro usuario
                bool existeIdentificacion = _validarIdentificacionUsuarioAD.ExisteIdentificacionExceptoUsuario(usuario.Identificacion, usuario.IdUsuario);
                if (existeIdentificacion)
                {
                    return false;
                }

                // Actualizar automáticamente la fecha de modificación
                usuario.FechaDeModificacion = DateTime.Now;

                int cantidad = _editarUsuarioAD.Editar(usuario);

                if (cantidad > 0)
                {
                    _bitacora.Registrar("USUARIOS", "UPDATE", null, usuario);
                }

                return cantidad > 0;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("USUARIOS", ex);
                throw;
            }
        }
    }
}
