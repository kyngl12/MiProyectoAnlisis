using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.AccesoADatos.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.ValidarIdentificacion;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioLN : IRegistrarUsuarioLN
    {
        private IRegistrarUsuarioAD _registrarUsuarioAD;
        private ValidarIdentificacionUsuarioAD _validarIdentificacionUsuarioAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public RegistrarUsuarioLN()
        {
            _registrarUsuarioAD = new RegistrarUsuarioAD();
            _validarIdentificacionUsuarioAD = new ValidarIdentificacionUsuarioAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public bool Registrar(UsuarioDto usuario)
        {
            try
            {
                // Validar campos obligatorios
                if (string.IsNullOrWhiteSpace(usuario.Nombres)) return false;
                if (string.IsNullOrWhiteSpace(usuario.PrimerApellido)) return false;
                if (string.IsNullOrWhiteSpace(usuario.SegundoApellido)) return false;
                if (string.IsNullOrWhiteSpace(usuario.Identificacion)) return false;
                if (string.IsNullOrWhiteSpace(usuario.CorreoElectronico)) return false;

                // Validar que la identificación no exista
                bool existeIdentificacion = _validarIdentificacionUsuarioAD.ExisteIdentificacion(usuario.Identificacion);
                if (existeIdentificacion)
                {
                    return false;
                }

                // Asignar valores automáticos
                usuario.FechaDeRegistro = DateTime.Now;
                usuario.FechaDeModificacion = null;
                usuario.Estado = true;

                int cantidad = _registrarUsuarioAD.Registrar(usuario);

                if (cantidad > 0)
                {
                    _bitacora.Registrar("USUARIOS", "INSERT", null, usuario);
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
