using System.ComponentModel.DataAnnotations;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad
{
    public class ImpuestoDto
    {
        public int IdImpuesto { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreImpuesto { get; set; }

        [Required(ErrorMessage = "El porcentaje es obligatorio.")]
        [Range(0, 100, ErrorMessage = "El valor ingresado no es valido. Las tasas deben ser porcentajes entre 0 y 100.")]
        public decimal Porcentaje { get; set; }

        public string ActualizadoPor { get; set; }
    }
}
