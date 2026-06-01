using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.ObtenerConfiguracionPorId
{
    public interface IObtenerConfiguracionPorIdAD
    {
        ConfiguracionComercioDto Obtener(int id);
    }
}
