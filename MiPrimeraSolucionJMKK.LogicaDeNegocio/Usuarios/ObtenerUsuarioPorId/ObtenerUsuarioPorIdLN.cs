using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.AccesoADatos.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.AccesoADatos.Usuarios.ObtenerUsuarioPorId;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId
{
    public class ObtenerUsuarioPorIdLN : IObtenerUsuarioPorIdLN
    {
        private ObtenerUsuarioPorIdAD _obtenerUsuarioPorIdAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public ObtenerUsuarioPorIdLN()
        {
            _obtenerUsuarioPorIdAD = new ObtenerUsuarioPorIdAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public UsuarioDto Obtener(int id)
        {
            try
            {
                UsuarioDto usuario = _obtenerUsuarioPorIdAD.ObtenerPorId(id);
                return usuario;
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("USUARIOS", ex);
                throw;
            }
        }
    }
}
