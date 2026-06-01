using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Entidades
{
    [Table("BITACORA")]
    public class BitacoraEntidad
    {
        [Key]
        [Column("ID_EVENTO")]
        public int IdEvento { get; set; }

        [Column("TABLA_DE_EVENTO")]
        public string TablaDeEvento { get; set; }

        [Column("TIPO_DE_EVENTO")]
        public string TipoDeEvento { get; set; }

        [Column("FECHA_DE_EVENTO")]
        public DateTime FechaDeEvento { get; set; }

        [Column("DESCRIPCION_DE_EVENTO")]
        public string DescripcionDeEvento { get; set; }

        [Column("STACK_TRACE")]
        public string StackTrace { get; set; }

        [Column("DATOS_ANTERIORES")]
        public string DatosAnteriores { get; set; }

        [Column("DATOS_POSTERIORES")]
        public string DatosPosteriores { get; set; }
    }
}
