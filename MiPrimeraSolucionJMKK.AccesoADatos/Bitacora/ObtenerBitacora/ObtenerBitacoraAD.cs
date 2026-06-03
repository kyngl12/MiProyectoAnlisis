using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;

namespace GestionPubRock.AccesoADatos.Bitacora.ObtenerBitacora
{
    public class ObtenerBitacoraAD
    {
        private readonly string _connectionString;

        public ObtenerBitacoraAD(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<BitacoraEventoDto> ObtenerBitacora()
        {
            List<BitacoraEventoDto> lista = new List<BitacoraEventoDto>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT * FROM BITACORA 
                 ORDER BY FECHA_DE_EVENTO DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new BitacoraEventoDto
                    {
                        IdEvento = Convert.ToInt32(reader["ID_EVENTO"]),
                        TablaDeEvento = reader["TABLA_DE_EVENTO"].ToString(),
                        TipoDeEvento = reader["TIPO_DE_EVENTO"].ToString(),
                        FechaDeEvento = Convert.ToDateTime(reader["FECHA_DE_EVENTO"]),
                        DescripcionDeEvento = reader["DESCRIPCION_DE_EVENTO"].ToString(),
                        StackTrace = reader["STACK_TRACE"].ToString(),
                        DatosAnteriores = reader["DATOS_ANTERIORES"]?.ToString(),
                        DatosPosteriores = reader["DATOS_POSTERIORES"]?.ToString()
                    });
                }
            }

            return lista;
        }
    }
}
