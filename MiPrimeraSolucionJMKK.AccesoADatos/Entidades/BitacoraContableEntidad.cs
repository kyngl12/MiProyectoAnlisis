using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    /// <summary>
    /// Mapea PUBROCK_BITACORA_TB: bitacora de movimientos contables
    /// (CON-006), independiente de la bitacora tecnica general
    /// (tabla BITACORA) usada para el registro de excepciones.
    /// </summary>
    [Table("PUBROCK_BITACORA_TB")]
    public class BitacoraContableEntidad
    {
        [Key]
        [Column("ID_BITACORA")]
        public int IdBitacora { get; set; }

        [Column("FECHA_HORA")]
        public DateTime FechaHora { get; set; }

        [Column("ACCION_REALIZADA")]
        public string AccionRealizada { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("CEDULA")]
        public string Cedula { get; set; }

        [Column("MODULO")]
        public string Modulo { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
    }
}
