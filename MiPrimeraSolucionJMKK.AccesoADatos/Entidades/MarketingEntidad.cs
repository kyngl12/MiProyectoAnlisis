using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_MARKETING_TB")]
    public class MarketingEntidad
    {
        [Key]
        [Column("ID_PUBLICACION")]
        public int IdPublicacion { get; set; }

        [Column("TITULO")]
        public string Titulo { get; set; }

        [Column("TIPO_CONTENIDO")]
        public string TipoContenido { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("FECHA_INICIO")]
        public DateTime FechaInicio { get; set; }

        [Column("FECHA_FIN")]
        public DateTime FechaFinalizacion { get; set; }

        [Column("PRECIO")]
        public decimal? Precio { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }

        [Column("FECHA_REGISTRO")]
        public DateTime FechaRegistro { get; set; }
    }
}