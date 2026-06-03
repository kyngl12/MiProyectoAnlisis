using GestionPubRock.Abstracciones.AccesoADatos.Cajas.ObtenerCajaPorId;
using GestionPubRock.AccesoADatos.Cajas.ObtenerCajaPorId;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCajaPorId;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Cajas.ObtenerCajaPorId
{
    public class ObtenerCajaPorIdLN : IObtenerCajaPorIdLN
    {
        private IObtenerCajaPorIdAD _obtenerCajaPorIdAD;

        public ObtenerCajaPorIdLN()
        {
            _obtenerCajaPorIdAD = new ObtenerCajaPorIdAD();
        }

        public CajaDto Obtener(int idCaja)
        {
            return _obtenerCajaPorIdAD.Obtener(idCaja);
        }
    }
}