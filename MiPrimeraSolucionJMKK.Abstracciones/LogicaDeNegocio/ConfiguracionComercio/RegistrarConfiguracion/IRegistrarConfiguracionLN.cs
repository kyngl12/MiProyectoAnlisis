using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.RegistrarConfiguracion
{
    public interface IRegistrarConfiguracionLN
    {
        bool Registrar(ConfiguracionComercioDto configuracion);
    }
}
