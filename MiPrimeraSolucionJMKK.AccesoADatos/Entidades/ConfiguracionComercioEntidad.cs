using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("CONFIGURACION_COMERCIO")]
    public class ConfiguracionComercioEntidad
    {
        [Key]
        [Column("ID_CONFIGURACION")]
        public int IdConfiguracion { get; set; }

        [Column("ID_COMERCIO")]
        public int IdComercio { get; set; }

        [Column("TIPO_CONFIGURACION")]
        public int TipoConfiguracion { get; set; }

        [Column("COMISION")]
        public int Comision { get; set; }

        [Column("FECHA_DE_REGISTRO")]
        public DateTime FechaDeRegistro { get; set; }

        [Column("FECHA_DE_MODIFICACION")]
        public DateTime? FechaDeModificacion { get; set; }

        [Column("ESTADO")]
        public bool Estado { get; set; }
    }
}
