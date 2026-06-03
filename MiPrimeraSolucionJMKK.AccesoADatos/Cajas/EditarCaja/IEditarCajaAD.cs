using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;

namespace GestionPubRock.Abstracciones.AccesoADatos.Cajas.EditarCaja
{
    public interface IEditarCajaAD
    {
        int Editar(CajaDto laCaja);
    }
}