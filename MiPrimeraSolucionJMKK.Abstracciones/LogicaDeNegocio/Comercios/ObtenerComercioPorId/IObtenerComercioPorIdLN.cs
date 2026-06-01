using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Comercios.ObtenerComercioPorId
{
    public interface IObtenerComercioPorIdLN
    {
        ComercioDto Obtener(int id);
    }
}
