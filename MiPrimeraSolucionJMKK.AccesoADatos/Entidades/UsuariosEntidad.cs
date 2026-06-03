using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_USUARIO_TB")]
    public class UsuariosEntidad
    {
        [Key]
        [Column("CEDULA")]
        public string Cedula { get; set; }

        [Column("NOMBRE")]
        public string Nombre { get; set; }

        [Column("APELLIDO_PATERNO")]
        public string ApellidoPaterno { get; set; }

        [Column("APELLIDO_MATERNO")]
        public string ApellidoMaterno { get; set; }

        [Column("FECHA_REGISTRO")]
        public DateTime FechaRegistro { get; set; }

        [Column("ID_TIPO_USUARIO")]
        public int IdTipoUsuario { get; set; }

        [Column("ID_ESTADO")]
        public int IdEstado { get; set; }

        [Column("CORREO")]
        public string Correo { get; set; }

        [Column("TELEFONO")]
        public string Telefono { get; set; }

        [Column("CONTRASENIA")]
        public string Contrasenia { get; set; }

        [ForeignKey("IdTipoUsuario")]
        public TipoUsuarioEntidad TipoUsuario { get; set; }
    }
}