using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Inventario
{
    public class MovimientoInventarioAD
    {
        private readonly Contexto _ctx;

        public MovimientoInventarioAD()
        {
            _ctx = new Contexto();
        }

        public string RegistrarMovimiento(int idProducto, int cantidad, string tipo, string motivo)
        {
            if (cantidad <= 0)
                return "CANTIDAD_INVALIDA";

            var stockActualObj = _ctx.Database.SqlQuery<int?>(
                "SELECT ISNULL(STOCK_ACTUAL,0) FROM PUBROCK_INVENTARIO_TB WHERE ID_PRODUCTO = @p0",
                idProducto
            );

            var stockActual = stockActualObj.AsEnumerable().FirstOrDefault() ?? 0;

            if (tipo == "SALIDA" && cantidad > stockActual)
                return $"STOCK_INSUFICIENTE|{stockActual}";

            int nuevoStock = stockActual;

            if (string.Equals(tipo, "ENTRADA", StringComparison.OrdinalIgnoreCase))
                nuevoStock += cantidad;
            else
                nuevoStock -= cantidad;

            // Actualizar inventario
            _ctx.Database.ExecuteSqlCommand(
                "UPDATE PUBROCK_INVENTARIO_TB SET STOCK_ACTUAL = @p0, FECHA_ULTIMA_ACTUALIZACION = GETDATE() WHERE ID_PRODUCTO = @p1",
                nuevoStock, idProducto
            );

            try
            {
                // Obtener ID_INVENTARIO asociado al producto
                var idInventario = _ctx.Database.SqlQuery<int?>("SELECT ID_INVENTARIO FROM PUBROCK_INVENTARIO_TB WHERE ID_PRODUCTO = @p0", idProducto).FirstOrDefault();

                // Obtener id tipo movimiento por descripcion
                var idTipoMovimiento = _ctx.Database.SqlQuery<int?>("SELECT ID_TIPO_MOVIMIENTO FROM PUBROCK_TIPO_MOVIMIENTO_INVENTARIO_TB WHERE DESCRIPCION = @p0", tipo).FirstOrDefault();

                if (!idTipoMovimiento.HasValue)
                {
                    // intentar crear el tipo si no existe (usar ID_ESTADO correspondiente a 'Activo')
                    var idEstadoActivo = _ctx.Database.SqlQuery<int?>("SELECT TOP 1 ID_ESTADO FROM PUBROCK_ESTADO_TB WHERE DESCRIPCION = 'Activo'").FirstOrDefault() ?? 1;
                    _ctx.Database.ExecuteSqlCommand("INSERT INTO PUBROCK_TIPO_MOVIMIENTO_INVENTARIO_TB (DESCRIPCION, ID_ESTADO) VALUES(@p0, @p1)", tipo, idEstadoActivo);
                    idTipoMovimiento = _ctx.Database.SqlQuery<int?>("SELECT TOP 1 ID_TIPO_MOVIMIENTO FROM PUBROCK_TIPO_MOVIMIENTO_INVENTARIO_TB WHERE DESCRIPCION = @p0", tipo).FirstOrDefault();
                }

                if (idInventario.HasValue && idTipoMovimiento.HasValue)
                {
                    _ctx.Database.ExecuteSqlCommand(@"
                        INSERT INTO PUBROCK_MOVIMIENTO_INVENTARIO_TB
                        (FECHA_MOVIMIENTO, CANTIDAD, OBSERVACION, ID_INVENTARIO, ID_TIPO_MOVIMIENTO, ID_ESTADO)
                        VALUES (GETDATE(), @p0, @p1, @p2, @p3, 1)",
                        cantidad, motivo ?? string.Empty, idInventario.Value, idTipoMovimiento.Value);
                }
            }
            catch { }

            var umbral = 5;

            if (nuevoStock <= umbral)
                return $"STOCK_BAJO|{nuevoStock}";

            return "OK";
        }
    }
}
