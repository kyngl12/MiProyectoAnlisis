using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Marketing
{
    public class MarketingDto
    {
        public int IdPublicacion { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de contenido es obligatorio.")]
        public string TipoContenido { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de finalización es obligatoria.")]
        public DateTime FechaFinalizacion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public int IdEstado { get; set; }

        public decimal? Precio { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string DescripcionTipoContenido { get; set; } = string.Empty;

        public string DescripcionEstado { get; set; } = string.Empty;
    }
}

