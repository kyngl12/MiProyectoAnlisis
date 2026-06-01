using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.ObtenerSINPESPorTelefonoCaja
{
    public interface IObtenerSINPESPorTelefonoCajaLN
    {
        List<SINPESDto> Obtener(string telefonoSINPE);
    }
}