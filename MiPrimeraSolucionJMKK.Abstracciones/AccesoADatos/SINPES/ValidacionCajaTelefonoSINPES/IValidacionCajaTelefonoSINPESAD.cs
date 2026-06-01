using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.SINPES.ValidacionCajaTelefonoAD
{
    public interface IValidacionCajaTelefonoSINPESAD
    {
        bool CajaValidaParaSINPE(string telefonoDestinatario);
    }
}
