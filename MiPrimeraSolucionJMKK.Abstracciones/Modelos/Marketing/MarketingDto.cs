using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Marketing
{
    public class MarketingDto
    {
        public int IdPublicacion { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título no puede superar 150 caracteres.")]
        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El tipo de contenido es obligatorio.")]
        [Display(Name = "Tipo de Contenido")]
        public int IdTipoContenido { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, ErrorMessage = "La descripción no puede superar 500 caracteres.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de finalización es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Finalización")]
        public DateTime FechaFinalizacion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [Display(Name = "Estado")]
        public int IdEstado { get; set; }

        [Display(Name = "Precio")]
        [Range(0, 9999999, ErrorMessage = "El precio debe ser mayor o igual a cero.")]
        public decimal? Precio { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string DescripcionTipoContenido { get; set; }

        public string DescripcionEstado { get; set; }
    }
}

