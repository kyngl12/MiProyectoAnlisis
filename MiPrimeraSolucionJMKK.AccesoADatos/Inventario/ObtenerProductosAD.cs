using System.Collections.Generic;
using System.Linq;
using GestionPubRock.AccesoADatos;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Productos;

namespace GestionPubRock.AccesoADatos.Inventario
{
    public class ObtenerProductosAD
    {
        private readonly Contexto _ctx;

        public ObtenerProductosAD()
        {
            _ctx = new Contexto();
        }

        public List<ProductoDto> ObtenerTodos()
        {
            string sql = @"
                SELECT
                    p.CODIGO_BARRAS AS Codigo,
                    p.NOMBRE_PRODUCTO AS Nombre,
                    ISNULL(c.NOMBRE_CATEGORIA, '') AS Categoria,
                    CAST(ISNULL(i.STOCK_ACTUAL, 0) AS DECIMAL(18,2)) AS Cantidad,
                    CAST(p.PRECIO_VENTA AS DECIMAL(18,2)) AS PrecioUnitario
                FROM PUBROCK_PRODUCTO_TB p
                LEFT JOIN PUBROCK_CATEGORIA_PRODUCTO_TB c ON p.ID_CATEGORIA_PRODUCTO = c.ID_CATEGORIA_PRODUCTO
                LEFT JOIN PUBROCK_INVENTARIO_TB i ON p.ID_PRODUCTO = i.ID_PRODUCTO
            ";

            return _ctx.Database.SqlQuery<ProductoDto>(sql).ToList();
        }

        public List<ProductoDto> ObtenerPorFiltro(string categoria, decimal? minCantidad, decimal? maxCantidad)
        {
            string sql = @"
                SELECT
                    p.CODIGO_BARRAS AS Codigo,
                    p.NOMBRE_PRODUCTO AS Nombre,
                    ISNULL(c.NOMBRE_CATEGORIA, '') AS Categoria,
                    CAST(ISNULL(i.STOCK_ACTUAL, 0) AS DECIMAL(18,2)) AS Cantidad,
                    CAST(p.PRECIO_VENTA AS DECIMAL(18,2)) AS PrecioUnitario
                FROM PUBROCK_PRODUCTO_TB p
                LEFT JOIN PUBROCK_CATEGORIA_PRODUCTO_TB c ON p.ID_CATEGORIA_PRODUCTO = c.ID_CATEGORIA_PRODUCTO
                LEFT JOIN PUBROCK_INVENTARIO_TB i ON p.ID_PRODUCTO = i.ID_PRODUCTO
                WHERE 1 = 1
            ";

            var parametros = new System.Collections.Generic.List<object>();

            if (!string.IsNullOrWhiteSpace(categoria))
            {
                sql += " AND c.NOMBRE_CATEGORIA = @p0";
                parametros.Add(categoria);
            }

            if (minCantidad.HasValue)
            {
                sql += " AND ISNULL(i.STOCK_ACTUAL,0) >= " + minCantidad.Value;
            }

            if (maxCantidad.HasValue)
            {
                sql += " AND ISNULL(i.STOCK_ACTUAL,0) <= " + maxCantidad.Value;
            }

            return parametros.Count > 0
                ? _ctx.Database.SqlQuery<ProductoDto>(sql, parametros.ToArray()).ToList()
                : _ctx.Database.SqlQuery<ProductoDto>(sql).ToList();
        }

        public List<ProductoDto> Buscar(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino)) return ObtenerTodos();

            string sql = @"
                SELECT
                    p.CODIGO_BARRAS AS Codigo,
                    p.NOMBRE_PRODUCTO AS Nombre,
                    ISNULL(c.NOMBRE_CATEGORIA, '') AS Categoria,
                    CAST(ISNULL(i.STOCK_ACTUAL, 0) AS DECIMAL(18,2)) AS Cantidad,
                    CAST(p.PRECIO_VENTA AS DECIMAL(18,2)) AS PrecioUnitario
                FROM PUBROCK_PRODUCTO_TB p
                LEFT JOIN PUBROCK_CATEGORIA_PRODUCTO_TB c ON p.ID_CATEGORIA_PRODUCTO = c.ID_CATEGORIA_PRODUCTO
                LEFT JOIN PUBROCK_INVENTARIO_TB i ON p.ID_PRODUCTO = i.ID_PRODUCTO
                WHERE LOWER(p.NOMBRE_PRODUCTO) LIKE @p0 OR LOWER(p.CODIGO_BARRAS) LIKE @p0
            ";

            string param = "%" + termino.ToLower() + "%";
            return _ctx.Database.SqlQuery<ProductoDto>(sql, param).ToList();
        }

        public System.Collections.Generic.List<string> ObtenerCategorias()
        {
            string sql = "SELECT NOMBRE_CATEGORIA FROM PUBROCK_CATEGORIA_PRODUCTO_TB WHERE NOMBRE_CATEGORIA IS NOT NULL";
            return _ctx.Database.SqlQuery<string>(sql).ToList();
        }
    }
}
