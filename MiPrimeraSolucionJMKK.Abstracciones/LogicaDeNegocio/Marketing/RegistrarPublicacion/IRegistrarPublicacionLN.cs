using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Marketing;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Marketing.RegistrarPublicacion
{
    public interface IRegistrarPublicacionLN
    {
        bool Registrar(MarketingDto laPublicacion);
    }
}
