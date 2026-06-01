using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.ObtenerCajaPorId;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCajaPorId;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;
using MiPrimeraSolucionJMKK.AccesoADatos.Cajas.ObtenerCajaPorId;

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