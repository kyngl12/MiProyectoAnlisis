using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.ObtenerCajasPorComercio
{
    public interface IObtenerCajasPorComercioAD
    {
        List<CajaDto> Obtener(int idComercio);
    }
}