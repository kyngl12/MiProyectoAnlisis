using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.ValidacionIdentificacionComercio
{
    public interface IValidacionIdentificacionComercioAD
    {
        bool ExisteIdentificacion(string identificacion);
    }
}
