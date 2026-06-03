using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.SINPES.ValidacionCajaTelefonoAD;
using System.Linq;

namespace GestionPubRock.AccesoADatos.SINPES.ValidacionCajaTelefonoAD
{
    public class ValidarCajaTelefonoSINPESAD : IValidacionCajaTelefonoSINPESAD
    {
        Contexto _elContexto;

        public ValidarCajaTelefonoSINPESAD()
        {
            _elContexto = new Contexto();
        }

        public bool CajaValidaParaSINPE(string telefonoDestinatario)
        {
            CajasEntidad caja = _elContexto.Cajas
                .Where(c => c.TelefonoSINPE == telefonoDestinatario)
                .FirstOrDefault();

            if (caja == null)
            {
                return false; 
            }

            if (!caja.Estado)
            {
                return false; 
            }

            return true; 
        }
    }
}