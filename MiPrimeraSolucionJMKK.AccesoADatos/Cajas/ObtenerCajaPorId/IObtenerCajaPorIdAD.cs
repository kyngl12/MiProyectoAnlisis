using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;

namespace GestionPubRock.Abstracciones.AccesoADatos.Cajas.ObtenerCajaPorId
{
    public interface IObtenerCajaPorIdAD
    {
        CajaDto Obtener(int idCaja);
    }
}