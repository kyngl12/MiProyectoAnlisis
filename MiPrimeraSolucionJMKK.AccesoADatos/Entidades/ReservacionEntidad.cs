using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_RESERVACION_TB")]
    public class ReservacionEntidad
    {
        [Key]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public bool Estado { get; set; }

        public string Cliente { get; set; }

        public string Observaciones { get; set; }
    }
}
