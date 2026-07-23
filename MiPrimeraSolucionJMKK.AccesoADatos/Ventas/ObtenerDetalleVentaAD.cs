using System;
using System.Data;
using System.Data.SqlClient;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace GestionPubRock.AccesoADatos.Ventas
{
    /// <summary>
    /// GPV-005: repositorio que invoca SP_PUBROCK_OBTENER_DETALLE_VENTA.
    /// </summary>
    public class ObtenerDetalleVentaAD : IObtenerDetalleVentaAD
    {
        private readonly Contexto _ctx;

        public ObtenerDetalleVentaAD()
        {
            _ctx = new Contexto();
        }

        public VentaResponseDto ObtenerDetalle(int idVenta)
        {
            var connection = (SqlConnection)_ctx.Database.Connection;
            bool debeCerrar = connection.State != ConnectionState.Open;

            try
            {
                if (debeCerrar) connection.Open();

                using (var comando = new SqlCommand("SP_PUBROCK_OBTENER_DETALLE_VENTA", connection))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@ID_VENTA", idVenta));

                    using (var reader = comando.ExecuteReader())
                    {
                        VentaResponseDto venta = null;

                        if (reader.Read())
                        {
                            venta = new VentaResponseDto
                            {
                                IdVenta = reader.GetInt32(reader.GetOrdinal("IdVenta")),
                                FechaVenta = reader.GetDateTime(reader.GetOrdinal("FechaVenta")),
                                IdCliente = reader.IsDBNull(reader.GetOrdinal("IdCliente")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("IdCliente")),
                                IdEmpleado = reader.GetInt32(reader.GetOrdinal("IdEmpleado")),
                                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                                Total = reader.GetDecimal(reader.GetOrdinal("Total")),
                                SubtotalGeneral = reader.GetDecimal(reader.GetOrdinal("SubtotalGeneral")),
                                TotalImpuestos = reader.GetDecimal(reader.GetOrdinal("TotalImpuestos")),
                                FechaAnulacion = reader.IsDBNull(reader.GetOrdinal("FechaAnulacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaAnulacion")),
                                MotivoAnulacion = reader.IsDBNull(reader.GetOrdinal("MotivoAnulacion")) ? null : reader.GetString(reader.GetOrdinal("MotivoAnulacion"))
                            };
                        }

                        if (venta == null) return null;

                        if (reader.NextResult())
                        {
                            while (reader.Read())
                            {
                                venta.Detalle.Add(new DetalleVentaResponseDto
                                {
                                    IdDetalleVenta = reader.GetInt32(reader.GetOrdinal("IdDetalleVenta")),
                                    IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                                    NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                                    Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                                    PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                                    Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal"))
                                });
                            }
                        }

                        return venta;
                    }
                }
            }
            finally
            {
                if (debeCerrar) connection.Close();
            }
        }
    }
}
