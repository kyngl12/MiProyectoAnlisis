using System;
using System.Configuration;
using GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora;
using GestionPubRock.AccesoADatos.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioLN : IRegistrarUsuarioLN
    {
        private IRegistrarUsuarioAD _registrarUsuarioAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public RegistrarUsuarioLN()
        {
            _registrarUsuarioAD = new RegistrarUsuarioAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public bool Registrar(UsuarioDto usuario)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(usuario.Cedula)) return false;
                if (string.IsNullOrWhiteSpace(usuario.Nombre)) return false;
                if (string.IsNullOrWhiteSpace(usuario.ApellidoPaterno)) return false;
                if (string.IsNullOrWhiteSpace(usuario.ApellidoMaterno)) return false;
                if (string.IsNullOrWhiteSpace(usuario.CorreoElectronico)) return false;
                if (string.IsNullOrWhiteSpace(usuario.Contrasenia)) return false;
                if (string.IsNullOrWhiteSpace(usuario.Telefono)) return false;
                if (usuario.IdTipoUsuario <= 0) return false;

                if (!EsFormatoEmailValido(usuario.CorreoElectronico)) return false;
                if (!EsFormatoTelefonoValido(usuario.Telefono)) return false;
                if (usuario.Contrasenia.Length < 6) return false;

                usuario.FechaRegistro = DateTime.Now;
                usuario.IdEstado = 1; 

                int cantidad = _registrarUsuarioAD.Registrar(usuario);

                if (cantidad > 0)
                    _bitacora.Registrar("PUBROCK_USUARIO_TB", "INSERT", null, usuario);

                return cantidad > 0;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("PUBROCK_USUARIO_TB", ex);
                throw;
            }
        }

        private bool EsFormatoEmailValido(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool EsFormatoTelefonoValido(string telefono)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(telefono, @"^\d{8,15}$");
        }
    }
}
