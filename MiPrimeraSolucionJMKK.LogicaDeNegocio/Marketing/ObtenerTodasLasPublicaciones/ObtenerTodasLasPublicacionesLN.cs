using System;
using System.Collections.Generic;
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
                return _obtenerTodasLasPublicacionesAD.ObtenerTodas();
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("PUBROCK_PUBLICACION_TB", ex);
                throw;
            }
        }
    }
}
