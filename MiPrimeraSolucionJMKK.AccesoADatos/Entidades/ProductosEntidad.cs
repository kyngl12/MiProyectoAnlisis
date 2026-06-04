using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionPubRock.AccesoADatos.Entidades
{
    [Table("PUBROCK_PRODUCTO_TB")]
    public class ProductosEntidad
    {
        [Key]
        [Column("ID_PRODUCTO")]
        public int IdProducto { get; set; }

        [Column("NOMBRE_PRODUCTO")]
        public string NombreProducto { get; set; }

        [Column("DESCRIPCION")]
        public string Descripcion { get; set; }

        [Column("CODIGO_BARRAS")]
        public string CodigoBarras { get; set; }

        [Column("PRECIO_VENTA")]
        public decimal PrecioVenta { get; set; }

        [Column("FECHA_VENCIMIENTO")]
        public System.DateTime? FechaVencimiento { get; set; }

        [Column("ID_CATEGORIA_PRODUCTO")]
        public int? IdCategoriaProducto { get; set; }

        [Column("ID_PROVEEDOR")]
        public int? IdProveedor { get; set; }

        [Column("ID_ESTADO")]
        public int? IdEstado { get; set; }
    }
}
