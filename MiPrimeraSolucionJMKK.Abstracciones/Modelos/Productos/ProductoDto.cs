using System.ComponentModel.DataAnnotations;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Productos
{
    public class ProductoDto
    {
        public string Codigo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        [Display(Name = "Categoría")]
        public string Categoria { get; set; }

        [Range(0, 9999999999.0, ErrorMessage = "La cantidad debe ser mayor o igual a 0.")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Range(0, 9999999999.0, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }
    }
}