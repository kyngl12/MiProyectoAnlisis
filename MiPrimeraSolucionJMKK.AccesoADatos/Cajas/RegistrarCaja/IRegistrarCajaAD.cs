using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;

namespace GestionPubRock.Abstracciones.AccesoADatos.Cajas.RegistrarCaja
{
    public interface IRegistrarCajaAD
    {
        int RegistrarCaja(CajaDto laCaja);
    }
}