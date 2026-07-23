using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_MOVIMIENTO_FINANCIERO_TB")]
    public class MovimientoFinancieroEntidad
    {
        [Key]
        [Column("ID_MOVIMIENTO_FINANCIERO")]
        public int IdMovimientoFinanciero { get; set; }

        [Column("FECHA_MOVIMIENTO")]
        public DateTime FechaMovimiento { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("MONTO")]
        public decimal Monto { get; set; }

        [Column("ID_TIPO_MOVIMIENTO_FINANCIERO")]
        public int IdTipoMovimientoFinanciero { get; set; }

        [Column("ID_VENTA")]
        public int? IdVenta { get; set; }

        [Column("ID_EMPLEADO")]
        public int IdEmpleado { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }

        [Column("CATEGORIA")]
        public string Categoria { get; set; }

        [Column("ES_FIJO")]
        public bool EsFijo { get; set; }

        [Column("NUMERO_COMPROBANTE")]
        public string NumeroComprobante { get; set; }
    }
}
