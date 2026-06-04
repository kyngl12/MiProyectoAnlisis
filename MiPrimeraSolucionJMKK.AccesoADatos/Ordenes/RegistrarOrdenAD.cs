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

                // Nota: la tabla PUBROCK_ORDEN_TB no contiene columna de proveedor en este esquema.
                // Insertar orden en la tabla usando las columnas definidas en el esquema
                string sqlInsert = "INSERT INTO PUBROCK_ORDEN_TB (FECHA_ORDEN, OBSERVACIONES, ID_VENTA, ID_ESTADO_ORDEN, ID_ESTADO) VALUES (GETDATE(), @p0, NULL, NULL, 1); SELECT SCOPE_IDENTITY();";
                var newId = _ctx.Database.SqlQuery<decimal?>(sqlInsert, orden.Observaciones ?? string.Empty).FirstOrDefault();
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
