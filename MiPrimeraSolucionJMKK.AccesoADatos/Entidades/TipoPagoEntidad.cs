using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_TIPO_PAGO_TB")]
    public class TipoPagoEntidad
    {
        [Key]
        [Column("ID_TIPO_PAGO")]
        public int IdTipoPago { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
    }
}
