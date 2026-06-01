using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Entidades
{
    [Table("SINPE")]
    public class SINPESEntidad
    {
        [Key]
        [Column("ID_SINPE")]
        public int IdSinpe { get; set; }

        [Column("TELEFONO_ORIGEN")]
        [Required]
        public string TelefonoOrigen { get; set; }

        [Column("NOMBRE_ORIGEN")]
        [Required]
        public string NombreOrigen { get; set; }

        [Column("TELEFONO_DESTINATARIO")]
        [Required]
        public string TelefonoDestinatario { get; set; }

        [Column("NOMBRE_DESTINATARIO")]
        [Required]
        public string NombreDestinatario { get; set; }

        [Column("MONTO")]
        [Required]
        public decimal Monto { get; set; }  

        [Column("FECHA_DE_REGISTRO")]
        [Required]
        public DateTime FechaDeRegistro { get; set; } 

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("ESTADO")]
        [Required]
        public bool Estado { get; set; }
    }
}