using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Marketing.ObtenerTodasLasPublicaciones
{
    public interface IObtenerTodasLasPublicacionesLN
    {
        List<MarketingDto> Obtener();
    }
}
