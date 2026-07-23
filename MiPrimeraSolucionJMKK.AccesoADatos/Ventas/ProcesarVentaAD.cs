using System;
using System.Data;
using System.Data.SqlClient;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace GestionPubRock.AccesoADatos.Ventas
{
    /// <summary>
    /// GPV-002 / GPV-006: repositorio que invoca SP_PUBROCK_PROCESAR_VENTA.
    /// </summary>
    public class ProcesarVentaAD : IProcesarVentaAD
    {
        private readonly Contexto _ctx;

        public ProcesarVentaAD()
        {
            _ctx = new Contexto();
        }

        public int Procesar(int idVenta, int idTipoPago, out ComprobanteVentaDto comprobante)
        {
            comprobante = null;
            int idFactura = 0;

            var connection = (SqlConnection)_ctx.Database.Connection;
            bool debeCerrar = connection.State != ConnectionState.Open;

            try
            {
                if (debeCerrar) connection.Open();

                int resultado;

                using (var comando = new SqlCommand("SP_PUBROCK_PROCESAR_VENTA", connection))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.Add(new SqlParameter("@ID_VENTA", idVenta));
                    comando.Parameters.Add(new SqlParameter("@ID_TIPO_PAGO", idTipoPago));

                    var idFacturaParam = new SqlParameter("@ID_FACTURA", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    comando.Parameters.Add(idFacturaParam);
                    comando.Parameters.Add(resultadoParam);

                    comando.ExecuteNonQuery();

                    idFactura = idFacturaParam.Value == DBNull.Value || idFacturaParam.Value == null ? 0 : Convert.ToInt32(idFacturaParam.Value);
                    resultado = Convert.ToInt32(resultadoParam.Value);
                }

                if (resultado == 1 && idFactura > 0)
                {
                    comprobante = ObtenerComprobante(connection, idFactura, idVenta);
                }

                return resultado;
            }
            finally
            {
                if (debeCerrar) connection.Close();
            }
        }

        private ComprobanteVentaDto ObtenerComprobante(SqlConnection connection, int idFactura, int idVenta)
        {
            ComprobanteVentaDto comprobante = null;

            using (var comando = new SqlCommand(@"
                SELECT F.ID_FACTURA, F.NUMERO_FACTURA, F.FECHA_FACTURA, F.MONTO_TOTAL,
                       V.ID_VENTA, V.ID_CLIENTE, TP.DESCRIPCION AS METODO_PAGO
                FROM PUBROCK_FACTURA_TB F
                INNER JOIN PUBROCK_VENTA_TB V ON V.ID_VENTA = F.ID_VENTA
                INNER JOIN PUBROCK_TIPO_PAGO_TB TP ON TP.ID_TIPO_PAGO = F.ID_TIPO_PAGO
                WHERE F.ID_FACTURA = @ID_FACTURA", connection))
            {
                comando.Parameters.Add(new SqlParameter("@ID_FACTURA", idFactura));

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        comprobante = new ComprobanteVentaDto
                        {
                            IdFactura = reader.GetInt32(reader.GetOrdinal("ID_FACTURA")),
                            NumeroFactura = reader.GetString(reader.GetOrdinal("NUMERO_FACTURA")),
                            FechaFactura = reader.GetDateTime(reader.GetOrdinal("FECHA_FACTURA")),
                            IdVenta = reader.GetInt32(reader.GetOrdinal("ID_VENTA")),
                            IdCliente = reader.IsDBNull(reader.GetOrdinal("ID_CLIENTE")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ID_CLIENTE")),
                            Total = reader.GetDecimal(reader.GetOrdinal("MONTO_TOTAL")),
                            MetodoPago = reader.GetString(reader.GetOrdinal("METODO_PAGO"))
                        };
                    }
                }
            }

            if (comprobante == null) return null;

            using (var comando = new SqlCommand(@"
                SELECT DV.ID_DETALLE_VENTA, DV.ID_PRODUCTO, P.NOMBRE_PRODUCTO, DV.CANTIDAD, DV.PRECIO_UNITARIO, DV.SUBTOTAL
                FROM PUBROCK_DETALLE_VENTA_TB DV
                INNER JOIN PUBROCK_PRODUCTO_TB P ON P.ID_PRODUCTO = DV.ID_PRODUCTO
                WHERE DV.ID_VENTA = @ID_VENTA AND DV.ID_ESTADO = 1", connection))
            {
                comando.Parameters.Add(new SqlParameter("@ID_VENTA", idVenta));

                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        comprobante.Detalle.Add(new DetalleVentaResponseDto
                        {
                            IdDetalleVenta = reader.GetInt32(reader.GetOrdinal("ID_DETALLE_VENTA")),
                            IdProducto = reader.GetInt32(reader.GetOrdinal("ID_PRODUCTO")),
                            NombreProducto = reader.GetString(reader.GetOrdinal("NOMBRE_PRODUCTO")),
                            Cantidad = reader.GetInt32(reader.GetOrdinal("CANTIDAD")),
                            PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PRECIO_UNITARIO")),
                            Subtotal = reader.GetDecimal(reader.GetOrdinal("SUBTOTAL"))
                        });
                    }
                }
            }

            return comprobante;
        }
    }
}
