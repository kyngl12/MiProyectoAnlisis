using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_IMPUESTO_TB")]
    public class ImpuestoEntidad
    {
        [Key]
        [Column("ID_IMPUESTO")]
        public int IdImpuesto { get; set; }

        [Column("NOMBRE_IMPUESTO")]
        public string NombreImpuesto { get; set; }

        [Column("PORCENTAJE")]
        public decimal Porcentaje { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
    }
}
