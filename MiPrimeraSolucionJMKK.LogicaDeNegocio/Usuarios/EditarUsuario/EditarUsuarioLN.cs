using System;
using System.Configuration;
using GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora;
using GestionPubRock.AccesoADatos.Usuarios.EditarUsuario;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.EditarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.EditarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.EditarUsuario
{
    public class EditarUsuarioLN : IEditarUsuarioLN
    {
        private IEditarUsuarioAD _editarUsuarioAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public EditarUsuarioLN()
        {
            _editarUsuarioAD = new EditarUsuarioAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public bool Editar(UsuarioDto usuario)
        {
            try
            {
                var camposFaltantes = new System.Collections.Generic.List<string>();

                if (string.IsNullOrWhiteSpace(usuario.Cedula)) camposFaltantes.Add("Cédula");
                if (string.IsNullOrWhiteSpace(usuario.Nombre)) camposFaltantes.Add("Nombre");
                if (string.IsNullOrWhiteSpace(usuario.ApellidoPaterno)) camposFaltantes.Add("Apellido Paterno");
                if (string.IsNullOrWhiteSpace(usuario.ApellidoMaterno)) camposFaltantes.Add("Apellido Materno");
                if (string.IsNullOrWhiteSpace(usuario.CorreoElectronico)) camposFaltantes.Add("Correo");
                if (string.IsNullOrWhiteSpace(usuario.Telefono)) camposFaltantes.Add("Teléfono");
                if (usuario.IdTipoUsuario <= 0) camposFaltantes.Add("Rol");

                if (camposFaltantes.Count > 0)
                    throw new System.ArgumentException("Campos obligatorios en blanco: " + string.Join(", ", camposFaltantes));

                if (!EsFormatoEmailValido(usuario.CorreoElectronico))
                    throw new System.ArgumentException("El formato del correo no es válido.");

                if (!EsFormatoTelefonoValido(usuario.Telefono))
                    throw new System.ArgumentException("El formato del teléfono no es válido.");

                int cantidad = _editarUsuarioAD.Editar(usuario);

                if (cantidad > 0)
                {
                    try { _bitacora.Registrar("PUBROCK_USUARIO_TB", "UPDATE", null, usuario); } catch { }
                    return true;
                }

                // AD devuelve códigos especiales para indicar la razón
                if (cantidad == -1) throw new System.ArgumentException("Error al editar. El usuario no se encuentra registrado en el sistema");
                if (cantidad == -2) throw new System.ArgumentException("El correo o cédula ya están registrados");
                if (cantidad == -3) throw new System.ArgumentException("El teléfono ya se encuentra registrado");

                throw new System.ArgumentException("Error al editar. Verifique los datos ingresados.");
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
            catch { return false; }
        }

        private bool EsFormatoTelefonoValido(string telefono)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(telefono, @"^\d{8,15}$");
        }
    }
}
