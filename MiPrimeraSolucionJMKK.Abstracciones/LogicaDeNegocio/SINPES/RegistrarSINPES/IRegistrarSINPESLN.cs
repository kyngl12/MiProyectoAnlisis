using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.SINPES.RegistrarSINPES
{
    public interface IRegistrarSINPESLN
    {
        bool Registrar(SINPESDto elPagoSINPESAGuardar);
    }
}