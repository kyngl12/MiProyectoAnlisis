using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;
using MiPrimeraSolucionJMKK.AccesoADatos.Bitacora.ObtenerBitacora;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
