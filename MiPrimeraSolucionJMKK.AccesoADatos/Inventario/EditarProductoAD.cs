using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Productos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestionPubRock.AccesoADatos.Inventario
{
    public class EditarProductoAD
    {
        private readonly Contexto _ctx;

        public EditarProductoAD()
        {
            _ctx = new Contexto();
        }

        public int Editar(ProductoDto producto)
        {
            try
            {

                if (producto.IdProducto <= 0)
                    return -4;

                if (string.IsNullOrWhiteSpace(producto.Nombre))
                    return -2; 

                if (producto.Cantidad < 0 || producto.PrecioUnitario < 0)
                    return -3;

                var entidad = _ctx.Productos
    .FirstOrDefault(p => p.IdProducto == producto.IdProducto);

                if (entidad == null)
                    return -5; 

                if (!string.IsNullOrWhiteSpace(producto.Codigo))
                {
                    var existeCodigo = _ctx.Database.SqlQuery<int>(
                        "SELECT COUNT(1) FROM PUBROCK_PRODUCTO_TB WHERE CODIGO_BARRAS = @p0 AND ID_PRODUCTO <> @p1",
                        producto.Codigo,
                        producto.IdProducto
                    ).FirstOrDefault() > 0;

                    if (existeCodigo)
                        return -1; 

                    entidad.CodigoBarras = producto.Codigo;
                }

                int? idCategoria = null;
                if (!string.IsNullOrWhiteSpace(producto.Categoria))
                {
                    var categoriaSql = _ctx.Database.SqlQuery<int?>(
                        "SELECT ID_CATEGORIA_PRODUCTO FROM PUBROCK_CATEGORIA_PRODUCTO_TB WHERE NOMBRE_CATEGORIA = @p0",
                        producto.Categoria
                    ).FirstOrDefault();

                    if (categoriaSql.HasValue)
                        idCategoria = categoriaSql.Value;
                }

                entidad.NombreProducto = producto.Nombre;
                entidad.Descripcion = producto.Nombre; // mantener descripción coherente (no sobreescribir con nombre de categoría)
                entidad.PrecioVenta = producto.PrecioUnitario;
                entidad.IdCategoriaProducto = idCategoria;

                int saved = _ctx.SaveChanges();

                try
                {
                    string sqlInv = @"
                        UPDATE PUBROCK_INVENTARIO_TB
                        SET STOCK_ACTUAL = @p0,
                            FECHA_ULTIMA_ACTUALIZACION = GETDATE()
                        WHERE ID_PRODUCTO = @p1";

                    int cantidadInt = Convert.ToInt32(System.Math.Floor(producto.Cantidad));

                    _ctx.Database.ExecuteSqlCommand(sqlInv, cantidadInt, producto.IdProducto);
                }
                catch { }

                return saved;
            }
            catch
            {
                throw;
            }
        }
    }
}
