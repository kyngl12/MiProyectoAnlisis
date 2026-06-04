using System.ComponentModel.DataAnnotations;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Productos
{
    public class ProductoDto
    {
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }

        [(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La categoria es obligatoria")]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; }

        [Range(0, 9999999999.0, ErrorMessage = "La cantidad debe ser mayor o igual a 0")]
        [Display(Name = "Cantidad")]
        public decimal Cantidad { get; set; }

        [Range(0, 9999999999.0, ErrorMessage = "El precio debe ser mayor o igual a 0")]
        [Display(Name = "PrecioUnitario")]
        public decimal PrecioUnitario { get; set; }
    }
}

/*

{

        public string Codigo { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; }
    public string Categoria { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    }
}
*/