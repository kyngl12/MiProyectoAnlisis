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

            var stockActual = _ctx.Database.SqlQuery<decimal>(
                "SELECT ISNULL(STOCK_ACTUAL,0) FROM PUBROCK_INVENTARIO_TB WHERE ID_PRODUCTO = @p0",
                idProducto
            ).FirstOrDefault();

            if (tipo == "SALIDA" && cantidad > stockActual)
                return $"STOCK_INSUFICIENTE|{stockActual}";

            decimal nuevoStock = stockActual;

            if (tipo == "ENTRADA")
                nuevoStock += cantidad;
            else
                nuevoStock -= cantidad;

            _ctx.Database.ExecuteSqlCommand(
                "UPDATE PUBROCK_INVENTARIO_TB SET STOCK_ACTUAL = @p0 WHERE ID_PRODUCTO = @p1",
                nuevoStock, idProducto
            );

            _ctx.Database.ExecuteSqlCommand(@"
                INSERT INTO PUBROCK_MOVIMIENTO_INVENTARIO_TB
                (ID_PRODUCTO, TIPO, CANTIDAD, MOTIVO, FECHA)
                VALUES (@p0, @p1, @p2, @p3, GETDATE())
            ", idProducto, tipo, cantidad, motivo);

            var umbral = 5; 

            if (nuevoStock <= umbral)
                return $"STOCK_BAJO|{nuevoStock}";

            return "OK";
        }
    }
}
