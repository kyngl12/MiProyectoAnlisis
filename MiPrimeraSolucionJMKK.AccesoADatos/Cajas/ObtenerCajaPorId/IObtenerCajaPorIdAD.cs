using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Cajas.ObtenerCajaPorId
{
    public interface IObtenerCajaPorIdAD
    {
        CajaDto Obtener(int idCaja);
    }
}