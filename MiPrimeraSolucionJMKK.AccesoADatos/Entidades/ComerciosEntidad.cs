using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("COMERCIOS")]
    public class ComerciosEntidad
    {
        [Key]
        [Column("ID_COMERCIO")]
        public int IdComercio { get; set; }

        [Column("IDENTIFICACION")]
        public string Identificacion { get; set; }

        [Column("TIPO_IDENTIFICACION")]
        public int TipoIdentificacion { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("TIPO_DE_COMERCIO")]
        public int TipoDeComercio { get; set; }

        [Column("TELEFONO")]
        public string Telefono { get; set; }

        [Column("CORREO_ELECTRONICO")]
        public string CorreoElectronico { get; set; }

        [Column("DIRECCION")]
        public string Direccion { get; set; }

        [Column("FECHA_DE_REGISTRO")]
        public DateTime FechaDeRegistro { get; set; }

        [Column("FECHA_DE_MODIFICACION")]
        public DateTime? FechaDeModificacion { get; set; }

        [Column("ESTADO")]
        public bool Estado { get; set; }
    }
}
