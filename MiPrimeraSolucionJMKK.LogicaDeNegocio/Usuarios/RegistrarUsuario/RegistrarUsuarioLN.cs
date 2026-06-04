using System;
using System.Configuration;
using System.Diagnostics;
using GestionPubRock.AccesoADatos.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.RegistrarUsuario
{
    public class RegistrarUsuarioLN : IRegistrarUsuarioLN
    {
        private IRegistrarUsuarioAD _registrarUsuarioAD;

        public RegistrarUsuarioLN()
        {
            _registrarUsuarioAD = new RegistrarUsuarioAD();
        }

        public bool Registrar(UsuarioDto usuario)
        {
            try
            {
                var camposFaltantes = new System.Collections.Generic.List<string>();

                if (string.IsNullOrWhiteSpace(usuario.Cedula)) camposFaltantes.Add("Cédula");
                if (string.IsNullOrWhiteSpace(usuario.Nombre)) camposFaltantes.Add("Nombre");
                if (string.IsNullOrWhiteSpace(usuario.ApellidoPaterno)) camposFaltantes.Add("Apellido Paterno");
                if (string.IsNullOrWhiteSpace(usuario.ApellidoMaterno)) camposFaltantes.Add("Apellido Materno");
                if (string.IsNullOrWhiteSpace(usuario.CorreoElectronico)) camposFaltantes.Add("Correo");
                if (string.IsNullOrWhiteSpace(usuario.Contrasenia)) camposFaltantes.Add("Contraseña");
                if (string.IsNullOrWhiteSpace(usuario.Telefono)) camposFaltantes.Add("Teléfono");
                if (usuario.IdTipoUsuario <= 0) camposFaltantes.Add("Rol");

                if (camposFaltantes.Count > 0)
                    throw new System.ArgumentException("Los siguientes campos obligatorios están vacíos: " + string.Join(", ", camposFaltantes));

                if (!EsFormatoEmailValido(usuario.CorreoElectronico))
                    throw new System.ArgumentException("El formato del correo no es válido.");

                if (!EsFormatoTelefonoValido(usuario.Telefono))
                    throw new System.ArgumentException("El formato del teléfono no es válido.");

                if (usuario.Contrasenia.Length < 6)
                    throw new System.ArgumentException("La contraseña debe tener al menos 6 caracteres.");

                usuario.FechaRegistro = DateTime.Now;
                usuario.IdEstado = 1;

                int cantidad = _registrarUsuarioAD.Registrar(usuario);
                if (cantidad > 0)
                {
                    return true;
                }

                if (cantidad == -1)
                    throw new System.ArgumentException("El usuario ya se encuentra registrado (cedula)");

                if (cantidad == -2)
                    throw new System.ArgumentException("El correo ya se encuentra registrado");

                if (cantidad == -3)
                    throw new System.ArgumentException("El teléfono ya se encuentra registrado");

                if (cantidad == -99)
                    throw new System.ArgumentException("Error en la base de datos al intentar registrar el usuario");

                return false;
            }
            catch (Exception ex)
            {
                // Registrar en archivo de log y propagar
                try { System.IO.File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "App_Data\\errors.log", DateTime.Now.ToString("s") + " - " + ex.ToString() + Environment.NewLine); } catch { }
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
