using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorId
{
    public interface IObtenerConfiguracionPorIdLN
    {
        ConfiguracionComercioDto Obtener(int id);
    }
}
