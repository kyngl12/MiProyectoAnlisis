using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;
using System.Collections.Generic;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorComercio
{
    public interface IObtenerConfiguracionPorComercioLN
    {
        List<ConfiguracionComercioDto> Obtener(int idComercio);
    }
}
