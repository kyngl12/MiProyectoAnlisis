using System;
using System.Data;
using System.Data.SqlClient;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas;

namespace GestionPubRock.AccesoADatos.Ventas
{
    /// <summary>
    /// GPV-003 / GPV-006: repositorio que invoca SP_PUBROCK_ANULAR_VENTA.
    /// </summary>
    public class AnularVentaAD : IAnularVentaAD
    {
        private readonly Contexto _ctx;

        public AnularVentaAD()
        {
            _ctx = new Contexto();
        }

        public int Anular(int idVenta, string motivoAnulacion)
        {
            var connection = (SqlConnection)_ctx.Database.Connection;
            bool debeCerrar = connection.State != ConnectionState.Open;

            try
            {
                if (debeCerrar) connection.Open();

                using (var comando = new SqlCommand("SP_PUBROCK_ANULAR_VENTA", connection))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.Add(new SqlParameter("@ID_VENTA", idVenta));
                    comando.Parameters.Add(new SqlParameter("@MOTIVO_ANULACION", (object)motivoAnulacion ?? DBNull.Value));

                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    comando.Parameters.Add(resultadoParam);

                    comando.ExecuteNonQuery();

                    return Convert.ToInt32(resultadoParam.Value);
                }
            }
            finally
            {
                if (debeCerrar) connection.Close();
            }
        }
    }
}
