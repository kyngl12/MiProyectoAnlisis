using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas
{
    public class CajaDto
    {
        public int IdCaja { get; set; }

        [Display(Name = "Comercio")]
        [Required]
        public int IdComercio { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        [MinLength(3)]
        [MaxLength(150)]
        public string Descripcion { get; set; }

        [Display(Name = "Teléfono SINPE")]
        [Required]
        [MinLength(8)]
        [MaxLength(10)]
        public string TelefonoSINPE { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime FechaDeRegistro { get; set; }

        [Display(Name = "Fecha de modificación")]
        public DateTime? FechaDeModificacion { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }

        public string DescripcionCorta
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Descripcion))
                {
                    return string.Empty;
                }

                return Descripcion.Length <= 10
                    ? Descripcion
                    : Descripcion.Substring(0, 10) + "...";
            }
        }
    }
}