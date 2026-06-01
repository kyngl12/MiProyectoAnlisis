using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.RegistrarConfiguracion
{
    public interface IRegistrarConfiguracionAD
    {
        bool Registrar(ConfiguracionComercioDto configuracion);
    }
}
