using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.RegistrarConfiguracion;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.RegistrarConfiguracion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;
using MiPrimeraSolucionJMKK.AccesoADatos.ConfiguracionComercio.RegistrarConfiguracion;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.ConfiguracionComercio.RegistrarConfiguracion
{
    public class RegistrarConfiguracionLN : IRegistrarConfiguracionLN
    {
        private readonly IRegistrarConfiguracionAD _registrarConfiguracionAD;

        public RegistrarConfiguracionLN()
        {
            _registrarConfiguracionAD = new RegistrarConfiguracionAD();
        }

        public bool Registrar(ConfiguracionComercioDto configuracion)
        {
            return _registrarConfiguracionAD.Registrar(configuracion);
        }
    }
}
