using System;
using System.ComponentModel.DataAnnotations;

namespace MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES
{
    public class SINPESDto
    {
        public int IdSinpe { get; set; }

        [Display(Name = "Teléfono origen")]
        [Required]
        [MinLength(8)]
        [MaxLength(10)]
        public string TelefonoOrigen { get; set; }

        [Display(Name = "Nombre origen")]
        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string NombreOrigen { get; set; }

        [Display(Name = "Teléfono destinatario")]
        [Required]
        [MinLength(8)]
        [MaxLength(10)]
        public string TelefonoDestinatario { get; set; }

        [Display(Name = "Nombre destinatario")]
        [Required]
        [MinLength(3)]
        [MaxLength(200)]
        public string NombreDestinatario { get; set; }

        [Display(Name = "Monto")]
        [Required]
        public decimal Monto { get; set; }

        [Display(Name = "Fecha de registro")]
        public DateTime FechaDeRegistro { get; set; }

        [Display(Name = "Descripción")]
        [MaxLength(50)]
        public string Descripcion { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }
    }
}