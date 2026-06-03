using GestionPubRock.AccesoADatos.Usuarios.ObtenerTodosLosTipoUsuarios;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Usuarios.ObtenerTodosLosTipoUsuarios;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerTodosLosTipoUsuarios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using System.Collections.Generic;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.ObtenerTodosLosTipoUsuarios
{
    public class ObtenerTodosLosTiposDeUsuarioLN : IObtenerTodosLosTiposDeUsuarioLN
    {
        private IObtenerTodosLosTiposDeUsuarioAD _obtenerTiposDeUsuarioAD;

        public ObtenerTodosLosTiposDeUsuarioLN()
        {
            _obtenerTiposDeUsuarioAD = new ObtenerTodosLosTiposDeUsuarioAD();
        }

        public List<TipoUsuarioDto> ObtenerTodos()
        {
            try
            {
                return _obtenerTiposDeUsuarioAD.ObtenerTodos();
            }
            catch
            {
                throw;
            }
        }
    }
}
