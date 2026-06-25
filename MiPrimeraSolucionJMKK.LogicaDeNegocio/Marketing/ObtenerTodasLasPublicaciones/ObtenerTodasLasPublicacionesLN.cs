using GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora;
using GestionPubRock.AccesoADatos.Marketing.ObtenerTodasLasPublicaciones;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Marketing.ObtenerTodasLasPublicaciones;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Marketing;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.LogicaDeNegocio.Marketing.ObtenerTodasLasPublicaciones
{
    public class ObtenerTodasLasPublicacionesLN : IObtenerTodasLasPublicacionesLN
    {
        private ObtenerTodasLasPublicacionesAD _obtenerTodasLasPublicacionesAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public ObtenerTodasLasPublicacionesLN()
        {
            _obtenerTodasLasPublicacionesAD = new ObtenerTodasLasPublicacionesAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public List<MarketingDto> Obtener()
        {
            try
            {
                return _obtenerTodasLasPublicacionesAD.ObtenerTodos();
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("PUBROCK_PUBLICACION_TB", ex);
                throw;
            }
        }
    }
}
