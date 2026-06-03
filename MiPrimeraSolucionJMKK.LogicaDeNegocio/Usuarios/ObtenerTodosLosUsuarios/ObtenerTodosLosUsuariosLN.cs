using System;
using System.Collections.Generic;
using System.Configuration;
using GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora;
using GestionPubRock.AccesoADatos.Usuarios.ObtenerTodosLosUsuarios;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios
{
    public class ObtenerTodosLosUsuariosLN : IObtenerTodosLosUsuariosLN
    {
        private ObtenerTodosLosUsuariosAD _obtenerTodosLosUsuariosAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public ObtenerTodosLosUsuariosLN()
        {
            _obtenerTodosLosUsuariosAD = new ObtenerTodosLosUsuariosAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public List<UsuarioDto> Obtener()
        {
            try
            {
                return _obtenerTodosLosUsuariosAD.ObtenerTodos();
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("PUBROCK_USUARIO_TB", ex);
                throw;
            }
        }
    }
}
