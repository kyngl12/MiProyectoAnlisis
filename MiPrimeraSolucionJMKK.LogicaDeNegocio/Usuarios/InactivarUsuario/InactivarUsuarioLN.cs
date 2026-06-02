using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.InactivarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.InactivarUsuario;
using MiPrimeraSolucionJMKK.AccesoADatos.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.InactivarUsuario;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.InactivarUsuario
{
    public class InactivarUsuarioLN : IInactivarUsuarioLN
    {
        private IInactivarUsuarioAD _inactivarUsuarioAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public InactivarUsuarioLN()
        {
            _inactivarUsuarioAD = new InactivarUsuarioAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public int Inactivar(string cedula)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cedula)) return -1;

                int resultado = _inactivarUsuarioAD.Inactivar(cedula);

                if (resultado > 0)
                    _bitacora.Registrar("PUBROCK_USUARIO_TB", "UPDATE-INACTIVAR", null, cedula);

                return resultado;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("PUBROCK_USUARIO_TB", ex);
                throw;
            }
        }
    }
}