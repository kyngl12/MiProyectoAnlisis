using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios
{
    public class ComercioDto
    {
        public int IdComercio { get; set; }

        [Display(Name = "Identificación")]
        [Required]
        [MinLength(8)]
        [MaxLength(30)]
        public string Identificacion { get; set; }

        [Display(Name = "Tipo de identificación")]
        [Required]
        public int TipoIdentificacion { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string Nombre { get; set; }

        [Display(Name = "Tipo de comercio")]
        [Required]
        public int TipoDeComercio { get; set; }

        [Display(Name = "Teléfono")]
        [Required]
        [MaxLength(20)]
        public string Telefono { get; set; }

        [Display(Name = "Correo electrónico")]
        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string CorreoElectronico { get; set; }

        [Display(Name = "Dirección")]
        [Required]
        [MinLength(3)]
        [MaxLength(500)]
        public string Direccion { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime FechaDeRegistro { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime? FechaDeModificacion { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }
    }
}
