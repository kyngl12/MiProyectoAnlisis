using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB")]
    public class TipoMovimientoFinancieroEntidad
    {
        [Key]
        [Column("ID_TIPO_MOVIMIENTO_FINANCIERO")]
        public int IdTipoMovimientoFinanciero { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
    }
}
