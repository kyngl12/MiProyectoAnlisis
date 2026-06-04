using System;
using System.Linq;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Productos;

namespace GestionPubRock.AccesoADatos.Inventario
{
    public class RegistrarProductoAD
    {
        private readonly Contexto _ctx;

        public RegistrarProductoAD()
        {
            _ctx = new Contexto();
        }

        public int Registrar(ProductoDto producto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(producto.Codigo) || string.IsNullOrWhiteSpace(producto.Nombre))
                    return -2; // campos obligatorios

                if (producto.Cantidad < 0 || producto.PrecioUnitario < 0)
                    return -3; // valores negativos

                // si no se proporciono codigo, generar uno automatico
                string codigoFinal = producto.Codigo;
                if (string.IsNullOrWhiteSpace(codigoFinal))
                {
                    // generar codigo simple: PROD + yyyyMMddHHmmss + random4
                    var rnd = new Random();
                    codigoFinal = "PROD" + DateTime.Now.ToString("yyyyMMddHHmmss") + rnd.Next(1000, 9999).ToString();
                }

                // asegurar unicidad del codigo generado o proporcionado
                int intentos = 0;
                while (intentos < 5)
                {
                    var count = _ctx.Database.SqlQuery<int>("SELECT COUNT(1) FROM PUBROCK_PRODUCTO_TB WHERE CODIGO_BARRAS = @p0", codigoFinal).FirstOrDefault();
                    if (count == 0) break;
                    // colision: generar sufijo aleatorio
                    codigoFinal = codigoFinal + "" + new Random().Next(100, 999).ToString();
                    intentos++;
                }
                // si tras reintentos aun existe, fallar
                var existeFinal = _ctx.Database.SqlQuery<int>("SELECT COUNT(1) FROM PUBROCK_PRODUCTO_TB WHERE CODIGO_BARRAS = @p0", codigoFinal).FirstOrDefault() > 0;
                if (existeFinal) return -1;

                // intentar resolver id de categoria por nombre
                int? idCategoria = null;
                if (!string.IsNullOrWhiteSpace(producto.Categoria))
                {
                    var categoriaSql = _ctx.Database.SqlQuery<int?>("SELECT ID_CATEGORIA_PRODUCTO FROM PUBROCK_CATEGORIA_PRODUCTO_TB WHERE NOMBRE_CATEGORIA = @p0", producto.Categoria).FirstOrDefault();
                    if (categoriaSql.HasValue) idCategoria = categoriaSql.Value;
                }

                var entidad = new ProductosEntidad
                {
                    NombreProducto = producto.Nombre,
                    Descripcion = producto.Categoria,
                    CodigoBarras = codigoFinal,
                    PrecioVenta = producto.PrecioUnitario,
                    FechaVencimiento = null,
                    IdCategoriaProducto = idCategoria,
                    IdProveedor = null,
                    IdEstado = 1
                };

                _ctx.Productos.Add(entidad);
                int saved = _ctx.SaveChanges();

                // opcional: crear registro en inventario con stock inicial
                try
                {
                    if (saved > 0 && producto.Cantidad > 0)
                    {
                        string sqlInv = "INSERT INTO PUBROCK_INVENTARIO_TB (STOCK_ACTUAL, STOCK_MINIMO, STOCK_MAXIMO, FECHA_ULTIMA_ACTUALIZACION, ID_PRODUCTO, ID_ESTADO) VALUES (@p0, 0, 0, GETDATE(), @p1, 1)";
                        // obtener el ultimo id insertado
                        // IDENT_CURRENT devuelve numeric/decimal, leer como decimal y convertir a int
                        var idProdDecimal = _ctx.Database.SqlQuery<decimal?>("SELECT IDENT_CURRENT('PUBROCK_PRODUCTO_TB')").FirstOrDefault();
                        int idProd = idProdDecimal.HasValue ? Convert.ToInt32(idProdDecimal.Value) : 0;
                        int cantidadInt = Convert.ToInt32(Math.Floor(producto.Cantidad));
                        _ctx.Database.ExecuteSqlCommand(sqlInv, cantidadInt, idProd);
                    }
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
