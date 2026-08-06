using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_VENTA_TB")]
    public class VentaEntidad
    {
        [Key]
        [Column("ID_VENTA")]
        public int IdVenta { get; set; }

        [Column("FECHA_VENTA")]
        public DateTime FechaVenta { get; set; }

        [Column("TOTAL_VENTA")]
        public decimal TotalVenta { get; set; }

        [Column("ID_CLIENTE")]
        public int? IdCliente { get; set; }

        [Column("ID_EMPLEADO")]
        public int IdEmpleado { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
    }
}
