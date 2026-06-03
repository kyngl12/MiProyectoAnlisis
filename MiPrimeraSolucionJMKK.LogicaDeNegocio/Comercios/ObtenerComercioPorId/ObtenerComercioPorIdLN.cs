using GestionPubRock.AccesoADatos.Comercios.ObtenerComercioPorId;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.ObtenerComercioPorId;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Comercios.ObtenerComercioPorId;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Comercios.ObtenerComercioPorId
{
    public class ObtenerComercioPorIdLN : IObtenerComercioPorIdLN
    {
        private IObtenerComercioPorIdAD _obtenerComercioPorIdAD;

        public ObtenerComercioPorIdLN()
        {
            _obtenerComercioPorIdAD = new ObtenerComercioPorIdAD();
        }

        public ComercioDto Obtener(int id)
        {
            ComercioDto elComercio = _obtenerComercioPorIdAD.Obtener(id);
            return elComercio;
        }
    }
}

