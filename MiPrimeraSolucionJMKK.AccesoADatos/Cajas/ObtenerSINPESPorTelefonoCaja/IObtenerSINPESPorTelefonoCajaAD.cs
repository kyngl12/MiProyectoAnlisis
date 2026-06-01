using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.ObtenerSINPESPorTelefonoCaja
{
    public interface IObtenerSINPESPorTelefonoCajaAD
    {
        List<SINPESDto> Obtener(string telefonoSINPE);
    }
}