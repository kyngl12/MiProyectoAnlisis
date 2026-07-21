using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_PROMOCION_TB")]
    public class PromocionEntidad
    {
        [Key]
        [Column("ID_PROMOCION")]
        public int IdPromocion { get; set; }

        [Column("NOMBRE_PROMOCION")]
        public string NombrePromocion { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("PORCENTAJE_DESCUENTO")]
        public decimal PorcentajeDescuento { get; set; }

        [Column("FECHA_INICIO")]
        public DateTime FechaInicio { get; set; }

        [Column("FECHA_FIN")]
        public DateTime FechaFin { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
    }
}