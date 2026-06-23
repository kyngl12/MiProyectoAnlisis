]using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Marketing.EditarPublicacion
{
    public interface IEditarPublicacionLN
    {
        bool Editar(PublicacionMarketingDto laPublicacion);
    }
}
