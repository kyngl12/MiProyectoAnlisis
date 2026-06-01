using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.ObtenerSINPESPorTelefonoCaja;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.ObtenerSINPESPorTelefonoCaja;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using MiPrimeraSolucionJMKK.AccesoADatos.Cajas.ObtenerSINPESPorTelefonoCaja;


namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Cajas.ObtenerSINPESPorTelefonoCaja
{
    public class ObtenerSINPESPorTelefonoCajaLN : IObtenerSINPESPorTelefonoCajaLN
    {
        private IObtenerSINPESPorTelefonoCajaAD _obtenerSINPESPorTelefonoCajaAD;

        public ObtenerSINPESPorTelefonoCajaLN()
        {
            _obtenerSINPESPorTelefonoCajaAD = new ObtenerSINPESPorTelefonoCajaAD();
        }

        public List<SINPESDto> Obtener(string telefonoSINPE)
        {
            return _obtenerSINPESPorTelefonoCajaAD.Obtener(telefonoSINPE);
        }
    }
}