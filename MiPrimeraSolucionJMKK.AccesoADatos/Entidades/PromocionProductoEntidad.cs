using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_PROMOCION_PRODUCTO_TB")]
    public class PromocionProductoEntidad
    {
        [Key, Column(Order = 0)]
        [Column("ID_PROMOCION")]
        public int IdPromocion { get; set; }

        [Key, Column(Order = 1)]
        [Column("ID_PRODUCTO")]
        public int IdProducto { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
    }
}
