using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Promocion
{
    public class PromocionDto
    {
        public int IdPromocion { get; set; }

        [Required(ErrorMessage = "El nombre de la promoción es obligatorio.")]
        [StringLength(100)]
        public string NombrePromocion { get; set; }

        [StringLength(255)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El porcentaje es obligatorio.")]
        [Range(1, 100, ErrorMessage = "El porcentaje ingresado no es válido.")]
        public decimal PorcentajeDescuento { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un producto.")]
        public int IdProducto { get; set; }

        public int IdEstado { get; set; }
    }
}
