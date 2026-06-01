using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.AccesoADatos.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.ObtenerTodosLosUsuarios;
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
                List<UsuarioDto> usuarios = _obtenerTodosLosUsuariosAD.ObtenerTodos();
                return usuarios;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("USUARIOS", ex);
                throw;
            }
        }
    }
}
