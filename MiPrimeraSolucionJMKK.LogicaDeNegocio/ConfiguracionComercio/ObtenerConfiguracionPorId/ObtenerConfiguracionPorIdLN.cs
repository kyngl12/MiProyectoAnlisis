using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.ObtenerConfiguracionPorId;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorId;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;
using MiPrimeraSolucionJMKK.AccesoADatos.ConfiguracionComercio.ObtenerConfiguracionPorId;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorId
{
    public class ObtenerConfiguracionPorIdLN : IObtenerConfiguracionPorIdLN
    {
        private readonly IObtenerConfiguracionPorIdAD _obtenerConfiguracionPorIdAD;

        public ObtenerConfiguracionPorIdLN()
        {
            _obtenerConfiguracionPorIdAD = new ObtenerConfiguracionPorIdAD();
        }

        public ConfiguracionComercioDto Obtener(int id)
        {
            return _obtenerConfiguracionPorIdAD.Obtener(id);
        }
    }
}
