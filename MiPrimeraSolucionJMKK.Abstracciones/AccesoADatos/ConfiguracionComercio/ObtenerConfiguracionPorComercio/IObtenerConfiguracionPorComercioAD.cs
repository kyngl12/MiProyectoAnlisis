using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;
using System.Collections.Generic;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.ObtenerConfiguracionPorComercio
{
    public interface IObtenerConfiguracionPorComercioAD
    {
        List<ConfiguracionComercioDto> Obtener(int idComercio);
    }
}
