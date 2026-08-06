using GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora;
using GestionPubRock.AccesoADatos.Promocion;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Promocion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Promocion;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.RegistrarBitacora;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.LogicaDeNegocio.Promocion
{
    public class ObtenerTodasLasPromocionesLN : IObtenerTodasLasPromocionesLN
    {
        private ObtenerTodasLasPromocionesAD _obtenerTodasLasPromocionesAD;
        private readonly IRegistrarBitacoraLN _bitacora;

        public ObtenerTodasLasPromocionesLN()
        {
            _obtenerTodasLasPromocionesAD = new ObtenerTodasLasPromocionesAD();

            string connectionString = ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            _bitacora = new RegistrarBitacoraLN(new RegistrarBitacoraAD(connectionString));
        }

        public List<PromocionDto> Obtener(string criterio = "", int? idEstado = null)
        {
            try
            {
                return _obtenerTodasLasPromocionesAD.Obtener(criterio, idEstado);
            }
            catch (Exception ex)
            {
                _bitacora.RegistrarError("PUBROCK_PROMOCION_TB", ex);
                throw;
            }
        }
    }
}
