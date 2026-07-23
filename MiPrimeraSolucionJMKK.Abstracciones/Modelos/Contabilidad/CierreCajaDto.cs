using System;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad
{
    public class CierreCajaDto
    {
        public int IdCierreCaja { get; set; }
        public DateTime FechaCierre { get; set; }
        public decimal TotalVentas { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
        public decimal BalanceFinal { get; set; }
        public decimal MontoContado { get; set; }
        public decimal Diferencia { get; set; }
        public string ResultadoCuadre { get; set; }
        public string RegistradoPor { get; set; }
    }
}
