using GestionPubRock.AccesoADatos.ConfiguracionComercio.EditarConfiguracion;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ConfiguracionComercio.EditarConfiguracion;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.EditarConfiguracion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.ConfiguracionComercio.EditarConfiguracion
{
    public class EditarConfiguracionLN : IEditarConfiguracionLN
    {
        private readonly IEditarConfiguracionAD _editarConfiguracionAD;

        public EditarConfiguracionLN()
        {
            _editarConfiguracionAD = new EditarConfiguracionAD();
        }

        public bool Editar(ConfiguracionComercioDto configuracion)
        {
            return _editarConfiguracionAD.Editar(configuracion);
        }
    }
}
