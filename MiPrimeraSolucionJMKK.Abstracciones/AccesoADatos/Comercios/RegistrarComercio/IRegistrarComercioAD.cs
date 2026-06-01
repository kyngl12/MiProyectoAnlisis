using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.RegstrarComercio
{
    public interface IRegistrarComercioAD
    {
        int RegistrarComercio(ComercioDto elComercio);
    }
}
