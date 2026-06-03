using System.Collections.Generic;
using GestionPubRock.Abstracciones.AccesoADatos.Cajas.ObtenerSINPESPorTelefonoCaja;
using GestionPubRock.AccesoADatos.Cajas.ObtenerSINPESPorTelefonoCaja;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.ObtenerSINPESPorTelefonoCaja;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;


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