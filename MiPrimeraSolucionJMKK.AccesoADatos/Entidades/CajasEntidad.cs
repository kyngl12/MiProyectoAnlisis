using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("CAJAS")]
    public class CajasEntidad
    {
        [Key]
        [Column("ID_CAJA")]
        public int IdCaja { get; set; }

        [Column("ID_COMERCIO")]
        public int IdComercio { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("TELEFONO_SINPE")]
        public string TelefonoSINPE { get; set; }

        [Column("FECHA_DE_REGISTRO")]
        public DateTime FechaDeRegistro { get; set; }

        [Column("FECHA_DE_MODIFICACION")]
        public DateTime? FechaDeModificacion { get; set; }

        [Column("ESTADO")]
        public bool Estado { get; set; }
    }
}