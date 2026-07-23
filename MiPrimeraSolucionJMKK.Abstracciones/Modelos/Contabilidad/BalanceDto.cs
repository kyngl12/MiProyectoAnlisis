using System;
using System.Collections.Generic;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad
{
    public class BalanceDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal UtilidadNeta { get; set; }
        public bool EsPerdida { get; set; }
        public List<BalanceCategoriaDto> Desglose { get; set; } = new List<BalanceCategoriaDto>();
    }
}
