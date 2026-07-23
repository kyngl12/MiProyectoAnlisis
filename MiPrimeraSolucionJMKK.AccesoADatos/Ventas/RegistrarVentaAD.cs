using System;
using System.Data;
using System.Data.SqlClient;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace GestionPubRock.AccesoADatos.Ventas
{
    /// <summary>
    /// GPV-001: repositorio que invoca SP_PUBROCK_REGISTRAR_VENTA.
    /// </summary>
    public class RegistrarVentaAD : IRegistrarVentaAD
    {
        private readonly Contexto _ctx;

        public RegistrarVentaAD()
        {
            _ctx = new Contexto();
        }

        public int Registrar(VentaRequestDto venta, out int idVentaCreada)
        {
            idVentaCreada = 0;

            var detalleTabla = new DataTable();
            detalleTabla.Columns.Add("ID_PRODUCTO", typeof(int));
            detalleTabla.Columns.Add("CANTIDAD", typeof(int));

            foreach (var linea in venta.Detalle)
            {
                detalleTabla.Rows.Add(linea.IdProducto, linea.Cantidad);
            }

            var connection = (SqlConnection)_ctx.Database.Connection;
            bool debeCerrar = connection.State != ConnectionState.Open;

            try
            {
                if (debeCerrar) connection.Open();

                using (var comando = new SqlCommand("SP_PUBROCK_REGISTRAR_VENTA", connection))
                {
                    comando.CommandType = CommandType.StoredProcedure;

                    comando.Parameters.Add(new SqlParameter("@ID_EMPLEADO", venta.IdEmpleado));
                    comando.Parameters.Add(new SqlParameter("@ID_CLIENTE", (object)venta.IdCliente ?? DBNull.Value));

                    var parametroDetalle = comando.Parameters.AddWithValue("@DETALLE", detalleTabla);
                    parametroDetalle.SqlDbType = SqlDbType.Structured;
                    parametroDetalle.TypeName = "PUBROCK_DETALLE_VENTA_TIPO_TB";

                    var idVentaParam = new SqlParameter("@ID_VENTA", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    var resultadoParam = new SqlParameter("@RESULTADO", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    comando.Parameters.Add(idVentaParam);
                    comando.Parameters.Add(resultadoParam);

                    comando.ExecuteNonQuery();

                    idVentaCreada = idVentaParam.Value == DBNull.Value || idVentaParam.Value == null ? 0 : Convert.ToInt32(idVentaParam.Value);
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
