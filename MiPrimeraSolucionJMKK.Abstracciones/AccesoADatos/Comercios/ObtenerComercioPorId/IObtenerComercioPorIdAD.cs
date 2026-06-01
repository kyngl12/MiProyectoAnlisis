using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Comercios.ObtenerComercioPorId
{
    public interface IObtenerComercioPorIdAD
    {
        ComercioDto Obtener(int id);
    }
}
