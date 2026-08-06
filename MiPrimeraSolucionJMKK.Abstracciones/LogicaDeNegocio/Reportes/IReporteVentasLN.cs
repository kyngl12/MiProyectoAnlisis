using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Reportes
{
    public interface IReporteVentasLN
    {
        ReporteVentasDto Generar(DateTime fechaInicio, DateTime fechaFin);

        ReporteVentasDto GenerarFiltrado(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado);
    }
}
