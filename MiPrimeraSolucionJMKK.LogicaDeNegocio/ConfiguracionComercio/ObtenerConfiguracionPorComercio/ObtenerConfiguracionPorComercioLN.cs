using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.ObtenerConfiguracionPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;
using MiPrimeraSolucionJMKK.AccesoADatos.ConfiguracionComercio.ObtenerConfiguracionPorComercio;
using System.Collections.Generic;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorComercio
{
    public class ObtenerConfiguracionPorComercioLN : IObtenerConfiguracionPorComercioLN
    {
        private readonly IObtenerConfiguracionPorComercioAD _obtenerConfiguracionPorComercioAD;

        public ObtenerConfiguracionPorComercioLN()
        {
            _obtenerConfiguracionPorComercioAD = new ObtenerConfiguracionPorComercioAD();
        }

        public List<ConfiguracionComercioDto> Obtener(int idComercio)
        {
            return _obtenerConfiguracionPorComercioAD.Obtener(idComercio);
        }
    }
}
