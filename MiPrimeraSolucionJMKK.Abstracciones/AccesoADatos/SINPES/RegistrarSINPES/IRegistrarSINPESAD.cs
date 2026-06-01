using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.SINPES.RegistrarSINPES
{
    public interface IRegistrarSINPESAD
    {
        int RegistrarSINPES(SINPESDto elPagoSINPES);
    }
}