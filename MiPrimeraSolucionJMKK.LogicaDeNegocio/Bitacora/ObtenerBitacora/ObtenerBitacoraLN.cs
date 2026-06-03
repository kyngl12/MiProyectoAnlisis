using GestionPubRock.AccesoADatos.Bitacora.ObtenerBitacora;
using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;
using System.Collections.Generic;

namespace MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.ObtenerBitacora
{
    public class ObtenerBitacoraLN
    {
        private readonly ObtenerBitacoraAD _bitacoraAD;

        public ObtenerBitacoraLN(ObtenerBitacoraAD bitacoraAD)
        {
            _bitacoraAD = bitacoraAD;
        }

        public List<BitacoraEventoDto> Ejecutar()
        {
            return _bitacoraAD.ObtenerBitacora();
        }
    }
}
