using System.Collections.Generic;
using System.Linq;
using GestionPubRock.Abstracciones.AccesoADatos.Cajas.ObtenerCajasPorComercio;
using GestionPubRock.AccesoADatos.Cajas.ObtenerCajasPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCajasPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;


namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Cajas.ObtenerCajasPorComercio
{
    public class ObtenerCajasPorComercioLN : IObtenerCajasPorComercioLN
    {
        private IObtenerCajasPorComercioAD _obtenerCajasPorComercioAD;

        public ObtenerCajasPorComercioLN()
        {
            _obtenerCajasPorComercioAD = new ObtenerCajasPorComercioAD();
        }

        public List<CajaDto> Obtener(int idComercio)
        {
            List<CajaDto> laListaDeCajas = _obtenerCajasPorComercioAD.Obtener(idComercio);

            laListaDeCajas = laListaDeCajas
                .OrderBy(caja => caja.Nombre)
                .ToList();

            return laListaDeCajas;
        }
    }
}