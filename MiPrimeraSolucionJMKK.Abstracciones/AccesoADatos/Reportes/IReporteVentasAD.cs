using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Reportes
{
    public interface IReporteVentasAD
    {
        ReporteVentasDto Generar(DateTime fechaInicio, DateTime fechaFin);

        // Método que acepta filtros opcionales: fecha inicio/fin, categoría, tipo de pago, cliente y estado
        ReporteVentasDto GenerarFiltrado(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado);
    }
}
