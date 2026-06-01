using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiPrimeraSolucionJMKK.AccesoADatos.Entidades
{
    [Table("USUARIOS")]
    public class UsuariosEntidad
    {
        [Key]
        [Column("ID_USUARIO")]
        public int IdUsuario { get; set; }

        [Column("ID_COMERCIO")]
        public int IdComercio { get; set; }

        [Column("ID_NET_USER")]
        public Guid? IdNetUser { get; set; }

        [Column("NOMBRES")]
        public string Nombres { get; set; }

        [Column("PRIMER_APELLIDO")]
        public string PrimerApellido { get; set; }

        [Column("SEGUNDO_APELLIDO")]
        public string SegundoApellido { get; set; }

        [Column("IDENTIFICACION")]
        public string Identificacion { get; set; }

        [Column("CORREO_ELECTRONICO")]
        public string CorreoElectronico { get; set; }

        [Column("FECHA_DE_REGISTRO")]
        public DateTime FechaDeRegistro { get; set; }

        [Column("FECHA_DE_MODIFICACION")]
        public DateTime? FechaDeModificacion { get; set; }

        [Column("ESTADO")]
        public bool Estado { get; set; }
    }
}