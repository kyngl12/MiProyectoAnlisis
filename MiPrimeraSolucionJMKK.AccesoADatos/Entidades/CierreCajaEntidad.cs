using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_CIERRE_CAJA_TB")]
    public class CierreCajaEntidad
    {
        [Key]
        [Column("ID_CIERRE_CAJA")]
        public int IdCierreCaja { get; set; }

        [Column("FECHA_CIERRE")]
        public DateTime FechaCierre { get; set; }

        [Column("TOTAL_VENTAS")]
        public decimal TotalVentas { get; set; }

        [Column("TOTAL_INGRESOS")]
        public decimal TotalIngresos { get; set; }

        [Column("TOTAL_EGRESOS")]
        public decimal TotalEgresos { get; set; }

        [Column("BALANCE_FINAL")]
        public decimal BalanceFinal { get; set; }

        [Column("ID_EMPLEADO")]
        public int IdEmpleado { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }

        [Column("MONTO_CONTADO")]
        public decimal MontoContado { get; set; }

        [Column("CEDULA_REGISTRO")]
        public string CedulaRegistro { get; set; }
    }
}
