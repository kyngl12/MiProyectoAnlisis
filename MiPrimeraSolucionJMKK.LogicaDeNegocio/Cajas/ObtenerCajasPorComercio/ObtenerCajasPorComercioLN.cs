using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.ObtenerCajasPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCajasPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;
using MiPrimeraSolucionJMKK.AccesoADatos.Cajas.ObtenerCajasPorComercio;


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