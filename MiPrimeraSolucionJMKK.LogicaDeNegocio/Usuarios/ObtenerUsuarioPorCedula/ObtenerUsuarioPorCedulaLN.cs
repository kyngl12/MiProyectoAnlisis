using GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora;
using GestionPubRock.AccesoADatos.Usuarios.ObtenerUsuarioPorId;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorCedula;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using System;
using System.Configuration;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId
{
    public class ObtenerUsuarioPorCedulaLN : IObtenerUsuarioPorCedulaLN
    {
        private ObtenerUsuarioPorCedulaAD _obtenerUsuarioPorCedulaAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public ObtenerUsuarioPorCedulaLN()
        {
            _obtenerUsuarioPorCedulaAD = new ObtenerUsuarioPorCedulaAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public UsuarioDto Obtener(string cedula)
        {
            try
            {
                return _obtenerUsuarioPorCedulaAD.ObtenerPorCedula(cedula);
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("PUBROCK_USUARIO_TB", ex);
                throw;
            }
        }
    }
}