using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_FACTURA_TB")]
    public class FacturaEntidad
    {
        [Key]
        [Column("ID_FACTURA")]
        public int IdFactura { get; set; }

        [Column("NUMERO_FACTURA")]
        public string NumeroFactura { get; set; }

        [Column("FECHA_FACTURA")]
        public DateTime FechaFactura { get; set; }

        [Column("MONTO_TOTAL")]
        public decimal MontoTotal { get; set; }

        [Column("ID_VENTA")]
        public int IdVenta { get; set; }

        [Column("ID_TIPO_PAGO")]
        public int IdTipoPago { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
    }
}
