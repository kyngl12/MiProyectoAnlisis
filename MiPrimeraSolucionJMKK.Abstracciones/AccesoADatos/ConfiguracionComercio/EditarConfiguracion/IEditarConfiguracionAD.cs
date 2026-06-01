using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.EditarConfiguracion
{
    public interface IEditarConfiguracionAD
    {
        bool Editar(ConfiguracionComercioDto configuracion);
    }
}
