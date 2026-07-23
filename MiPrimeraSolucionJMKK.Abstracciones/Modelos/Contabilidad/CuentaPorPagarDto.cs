using System;
using System.ComponentModel.DataAnnotations;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad
{
    public class CuentaPorPagarDto
    {
        public int IdCuentaPorPagar { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio.")]
        [StringLength(150)]
        public string Proveedor { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(255)]
        public string Descripcion { get; set; }

        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
        public DateTime FechaVencimiento { get; set; }

        public DateTime? FechaPago { get; set; }

        public string EstadoPago { get; set; }

        public bool EstaVencida { get; set; }

        public int DiasParaVencer { get; set; }

        public string RegistradoPor { get; set; }
    }
}
