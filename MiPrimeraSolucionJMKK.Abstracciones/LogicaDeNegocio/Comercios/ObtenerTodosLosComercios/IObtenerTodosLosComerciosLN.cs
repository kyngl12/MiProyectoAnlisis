using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Comercios.ObtenerTodosLosComercios
{
    public interface IObtenerTodosLosComerciosLN
    {
        List<ComercioDto> Obtener();
    }
}
