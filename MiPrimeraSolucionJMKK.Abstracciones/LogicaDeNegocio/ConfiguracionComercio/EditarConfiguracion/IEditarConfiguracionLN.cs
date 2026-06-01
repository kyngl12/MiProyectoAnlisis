using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.EditarConfiguracion
{
    public interface IEditarConfiguracionLN
    {
        bool Editar(ConfiguracionComercioDto configuracion);
    }
}
