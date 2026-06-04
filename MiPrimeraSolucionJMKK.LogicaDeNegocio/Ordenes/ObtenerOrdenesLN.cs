using System.Collections.Generic;
using GestionPubRock.AccesoADatos.Ordenes;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Ordenes
{
    public class ObtenerOrdenesLN
    {
        private readonly ObtenerOrdenesAD _ad;

        public ObtenerOrdenesLN()
        {
            _ad = new ObtenerOrdenesAD();
        }

        public List<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ordenes.OrdenListDto> ObtenerTodos()
        {
            return _ad.ObtenerTodos();
        }
    }
}
