using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_CUENTA_POR_PAGAR_TB")]
    public class CuentaPorPagarEntidad
    {
        [Key]
        [Column("ID_CUENTA_POR_PAGAR")]
        public int IdCuentaPorPagar { get; set; }

        [Column("PROVEEDOR")]
        public string Proveedor { get; set; }

        [Column("MONTO")]
        public decimal Monto { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("FECHA_REGISTRO")]
        public DateTime FechaRegistro { get; set; }

        [Column("FECHA_VENCIMIENTO")]
        public DateTime FechaVencimiento { get; set; }

        [Column("FECHA_PAGO")]
        public DateTime? FechaPago { get; set; }

        [Column("ESTADO_PAGO")]
        public string EstadoPago { get; set; }

        [Column("NOTIFICACION_ENVIADA")]
        public bool NotificacionEnviada { get; set; }

        [Column("ID_MOVIMIENTO_FINANCIERO")]
        public int? IdMovimientoFinanciero { get; set; }

        [Column("CEDULA_REGISTRO")]
        public string CedulaRegistro { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }
    }
}
