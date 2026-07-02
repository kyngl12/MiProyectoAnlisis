using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_RESERVACION_TB")]
    public class ReservacionEntidad
    {
        [Key]
        [Column("ID_RESERVACION")]
        public int Id { get; set; }

        [Column("FECHA_RESERVACION")]
        public DateTime FechaReservacion { get; set; }

        [Column("HORA_INICIO")]
        public TimeSpan? HoraInicio { get; set; }

        [Column("HORA_FIN")]
        public TimeSpan? HoraFin { get; set; }

        [Column("CANTIDAD_PERSONAS")]
        public int? CantidadPersonas { get; set; }

        [Column("OBSERVACIONES")]
        public string Observaciones { get; set; }

        [Column("ID_CLIENTE")]
        public int? IdCliente { get; set; }

        [Column("ID_MESA")]
        public int? IdMesa { get; set; }

        [Column("ID_ESTADO")]
        public int? IdEstado { get; set; }
    }
}
