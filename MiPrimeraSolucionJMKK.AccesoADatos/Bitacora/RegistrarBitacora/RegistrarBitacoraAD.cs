using System;
using System.Data;
using System.Data.SqlClient;
using MiPrimeraSolucionJMKK.Abstacciones.AccesoADatos.Bitacora.RegistrarBitacora;
using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;

namespace GestionPubRock.AccesoADatos.Bitacora.RegistrarBitacora
{
    public class RegistrarBitacoraAD : IRegistrarBitacoraAD
    {
        private readonly string _connectionString;

        public RegistrarBitacoraAD(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void RegistrarEvento(BitacoraEventoDto evento)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO BITACORA
                (TABLA_DE_EVENTO, TIPO_DE_EVENTO, FECHA_DE_EVENTO, DESCRIPCION_DE_EVENTO, STACK_TRACE, DATOS_ANTERIORES, DATOS_POSTERIORES)
                VALUES
                (@Tabla, @Tipo, @Fecha, @Descripcion, @Stack, @Antes, @Despues)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@Tabla", evento.TablaDeEvento);
                cmd.Parameters.AddWithValue("@Tipo", evento.TipoDeEvento);
                cmd.Parameters.AddWithValue("@Fecha", evento.FechaDeEvento);
                cmd.Parameters.AddWithValue("@Descripcion", evento.DescripcionDeEvento ?? "");
                cmd.Parameters.AddWithValue("@Stack", evento.StackTrace ?? "");
                cmd.Parameters.AddWithValue("@Antes", (object)evento.DatosAnteriores ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Despues", (object)evento.DatosPosteriores ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}