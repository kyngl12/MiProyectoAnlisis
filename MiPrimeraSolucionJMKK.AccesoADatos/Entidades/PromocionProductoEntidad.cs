using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_PROMOCION_PRODUCTO_TB")]
    public class PromocionProductoEntidad
    {
        [Key]
        [Column("ID_PROMOCION", Order = 0)]
        public int IdPromocion { get; set; }

        [Key]
        [Column("ID_PRODUCTO", Order = 1)]
        public int IdProducto { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
    }
}
