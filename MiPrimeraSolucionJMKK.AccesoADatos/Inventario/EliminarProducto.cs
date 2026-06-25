using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Inventario
{
    public class EliminarProductoAD
    {
        private readonly Contexto _ctx;

        public EliminarProductoAD()
        {
            _ctx = new Contexto();
        }

        public string Eliminar(int idProducto)
        {
            var existe = _ctx.Database.SqlQuery<int>(
                "SELECT COUNT(1) FROM PUBROCK_PRODUCTO_TB WHERE ID_PRODUCTO = @p0",
                idProducto
            ).FirstOrDefault();

            if (existe == 0)
            {
                return "NO_EXISTE";
            }

            var tieneOrdenes = _ctx.Database.SqlQuery<int>(
                @"
                SELECT COUNT(1)
                FROM PUBROCK_ORDEN_DETALLE_TB od
                INNER JOIN PUBROCK_ORDEN_TB o ON od.ID_ORDEN = o.ID_ORDEN
                WHERE od.ID_PRODUCTO = @p0
                AND o.ESTADO IN ('PENDIENTE','EN_PROCESO')
                ",
                idProducto
            ).FirstOrDefault();

            if (tieneOrdenes > 0)
            {
                return "TIENE_ORDENES";
            }

            _ctx.Database.ExecuteSqlCommand(
                "DELETE FROM PUBROCK_PRODUCTO_TB WHERE ID_PRODUCTO = @p0",
                idProducto
            );

            return "OK";
        }
    }
}
