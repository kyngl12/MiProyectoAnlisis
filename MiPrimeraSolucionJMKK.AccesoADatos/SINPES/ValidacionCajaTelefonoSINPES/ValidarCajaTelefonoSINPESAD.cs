using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.SINPES.ValidacionCajaTelefonoAD;
using MiPrimeraSolucionJMKK.AccesoADatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.AccesoADatos.SINPES.ValidacionCajaTelefonoAD
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