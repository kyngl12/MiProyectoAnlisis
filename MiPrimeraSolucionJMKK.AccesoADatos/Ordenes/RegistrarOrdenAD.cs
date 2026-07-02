using System;
using System.Linq;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ordenes;

namespace GestionPubRock.AccesoADatos.Ordenes
{
    public class RegistrarOrdenAD
    {
        private readonly Contexto _ctx;

        public RegistrarOrdenAD()
        {
            _ctx = new Contexto();
        }

        public int Registrar(OrdenDto orden)
        {
            try
            {
                if (orden == null) return -4;
                if (orden.Cantidad <= 0) return -3;

                // Asegurar existencia de un ID_VENTA válido (crear uno mínimo si no existe)
                var idVenta = _ctx.Database.SqlQuery<int?>("SELECT TOP 1 ID_VENTA FROM PUBROCK_VENTA_TB ORDER BY ID_VENTA").FirstOrDefault();
                if (!idVenta.HasValue)
                {
                    // intentar crear una venta mínima usando el primer empleado disponible
                    var idEmpleado = _ctx.Database.SqlQuery<int?>("SELECT TOP 1 ID_EMPLEADO FROM PUBROCK_EMPLEADO_TB ORDER BY ID_EMPLEADO").FirstOrDefault();
                    if (!idEmpleado.HasValue) return -7; // no hay empleado para asociar la venta

                    var idEstadoActivo = _ctx.Database.SqlQuery<int?>("SELECT TOP 1 ID_ESTADO FROM PUBROCK_ESTADO_TB WHERE DESCRIPCION = 'Activo'").FirstOrDefault() ?? 1;
                    var sqlVenta = "INSERT INTO PUBROCK_VENTA_TB (FECHA_VENTA, TOTAL_VENTA, ID_CLIENTE, ID_EMPLEADO, ID_ESTADO) VALUES (GETDATE(), 0, NULL, @p0, @p1); SELECT SCOPE_IDENTITY();";
                    var newVenta = _ctx.Database.SqlQuery<decimal?>(sqlVenta, idEmpleado.Value, idEstadoActivo).FirstOrDefault();
                    if (!newVenta.HasValue) return -8;
                    idVenta = Convert.ToInt32(newVenta.Value);
                }

                // Asegurar existencia de un estado de orden (ej. 'Pendiente')
                var idEstadoOrden = _ctx.Database.SqlQuery<int?>("SELECT TOP 1 ID_ESTADO_ORDEN FROM PUBROCK_ESTADO_ORDEN_TB ORDER BY ID_ESTADO_ORDEN").FirstOrDefault();
                if (!idEstadoOrden.HasValue)
                {
                    // crear estado orden por defecto
                    var sqlCrearEstadoOrden = "INSERT INTO PUBROCK_ESTADO_ORDEN_TB (DESCRIPCION, ID_ESTADO) VALUES (@p0, (SELECT ID_ESTADO FROM PUBROCK_ESTADO_TB WHERE DESCRIPCION = 'Activo')); SELECT SCOPE_IDENTITY();";
                    var newEstadoOrden = _ctx.Database.SqlQuery<decimal?>(sqlCrearEstadoOrden, "Pendiente").FirstOrDefault();
                    if (!newEstadoOrden.HasValue) return -9;
                    idEstadoOrden = Convert.ToInt32(newEstadoOrden.Value);
                }

                var idEstadoActivoOrden = _ctx.Database.SqlQuery<int?>("SELECT TOP 1 ID_ESTADO FROM PUBROCK_ESTADO_TB WHERE DESCRIPCION = 'Activo'").FirstOrDefault() ?? 1;

                // Insertar orden en la tabla usando las columnas definidas en el esquema
                string sqlInsert = "INSERT INTO PUBROCK_ORDEN_TB (FECHA_ORDEN, OBSERVACIONES, ID_VENTA, ID_ESTADO_ORDEN, ID_ESTADO) VALUES (GETDATE(), @p0, @p1, @p2, @p3); SELECT SCOPE_IDENTITY();";
                var newId = _ctx.Database.SqlQuery<decimal?>(sqlInsert, orden.Observaciones ?? string.Empty, idVenta.Value, idEstadoOrden.Value, idEstadoActivoOrden).FirstOrDefault();
                if (newId == null) return 0;

                int idOrden = Convert.ToInt32(newId.Value);

                // Insertar detalle
                string sqlDetalle = "INSERT INTO PUBROCK_DETALLE_ORDEN_TB (ID_ORDEN, ID_PRODUCTO, CANTIDAD, OBSERVACION, ID_ESTADO) VALUES (@p0, @p1, @p2, @p3, 1)";
                // Verificar producto existe
                var producto = _ctx.Database.SqlQuery<int?>("SELECT ID_PRODUCTO FROM PUBROCK_PRODUCTO_TB WHERE ID_PRODUCTO = @p0", orden.IdProducto).FirstOrDefault();
                if (producto == null) return -6;

                // Convertir cantidad a entero antes de pasar al comando SQL (DB espera INT)
                int cantidadInt = Convert.ToInt32(orden.Cantidad);
                int res = _ctx.Database.ExecuteSqlCommand(sqlDetalle, idOrden, orden.IdProducto, cantidadInt, orden.Observaciones ?? string.Empty);

                return res > 0 ? idOrden : 0;
            }
            catch
            {
                throw;
            }
        }
    }
}
