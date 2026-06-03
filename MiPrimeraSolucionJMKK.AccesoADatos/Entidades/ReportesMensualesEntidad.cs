using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("REPORTES_MENSUALES")]
    public class ReportesMensualesEntidad
    {
        [Key]
        [Column("ID_REPORTE")]
        public int IdReporte { get; set; }

        [Column("ID_COMERCIO")]
        public int IdComercio { get; set; }

        [Column("NOMBRE_COMERCIO")]
        public string NombreComercio { get; set; }

        [Column("CANTIDAD_DE_CAJAS")]
        public int CantidadDeCajas { get; set; }

        [Column("MONTO_TOTAL_RECAUDADO")]
        public decimal MontoTotalRecaudado { get; set; }

        [Column("CANTIDAD_DE_SINPES")]
        public int CantidadDeSINPES { get; set; }

        [Column("MONTO_TOTAL_COMISION")]
        public decimal MontoTotalComision { get; set; }

        [Column("FECHA_DEL_REPORTE")]
        public DateTime FechaDelReporte { get; set; }
    }
}
