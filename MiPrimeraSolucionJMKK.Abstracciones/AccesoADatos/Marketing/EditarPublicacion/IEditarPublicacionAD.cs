using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Marketing.EditarPublicacion
{
    public interface IEditarPublicacionAD
    {
        int Editar(MarketingDto laPublicacion);
    }
}
