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

            // Comprobar si existen detalles de orden que usen el producto y cuya orden esté en estado "Pendiente" o "En proceso"
            var tieneOrdenes = _ctx.Database.SqlQuery<int>(
                @"
                SELECT COUNT(1)
                FROM PUBROCK_DETALLE_ORDEN_TB od
                INNER JOIN PUBROCK_ORDEN_TB o ON od.ID_ORDEN = o.ID_ORDEN
                INNER JOIN PUBROCK_ESTADO_ORDEN_TB eo ON o.ID_ESTADO_ORDEN = eo.ID_ESTADO_ORDEN
                WHERE od.ID_PRODUCTO = @p0
                AND eo.DESCRIPCION IN ('PENDIENTE','EN_PROCESO')
                ",
                idProducto
            ).FirstOrDefault();

            if (tieneOrdenes > 0)
            {
                return "TIENE_ORDENES";
            }

            // Soft delete: marcar estado como 'Inactivo' usando la tabla de estados
            // Nota: PUBROCK_PRODUCTO_TB no contiene FECHA_ULTIMA_ACTUALIZACION en el esquema real, solo actualizar ID_ESTADO
            _ctx.Database.ExecuteSqlCommand(
                "UPDATE PUBROCK_PRODUCTO_TB SET ID_ESTADO = (SELECT ID_ESTADO FROM PUBROCK_ESTADO_TB WHERE DESCRIPCION = 'Inactivo') WHERE ID_PRODUCTO = @p0",
                idProducto
            );

            // Si existe registro en inventario, actualizar su FECHA_ULTIMA_ACTUALIZACION
            try
            {
                _ctx.Database.ExecuteSqlCommand(
                    "UPDATE PUBROCK_INVENTARIO_TB SET FECHA_ULTIMA_ACTUALIZACION = GETDATE() WHERE ID_PRODUCTO = @p0",
                    idProducto
                );
            }
            catch { }

            return "OK";
        }
    }
}
