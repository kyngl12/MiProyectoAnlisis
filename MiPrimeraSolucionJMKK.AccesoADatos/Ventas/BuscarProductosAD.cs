using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;

namespace GestionPubRock.AccesoADatos.Ventas
{
    /// <summary>
    /// GPV-004: repositorio que invoca SP_PUBROCK_BUSCAR_PRODUCTOS.
    /// </summary>
    public class BuscarProductosAD : IBuscarProductosAD
    {
        private readonly Contexto _ctx;

        public BuscarProductosAD()
        {
            _ctx = new Contexto();
        }

        public List<BuscarProductoResponseDto> Buscar(string textoBusqueda)
        {
            var resultado = new List<BuscarProductoResponseDto>();

            var connection = (SqlConnection)_ctx.Database.Connection;
            bool debeCerrar = connection.State != ConnectionState.Open;

            try
            {
                if (debeCerrar) connection.Open();

                using (var comando = new SqlCommand("SP_PUBROCK_BUSCAR_PRODUCTOS", connection))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@TEXTO_BUSQUEDA", (object)textoBusqueda ?? DBNull.Value));

                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultado.Add(new BuscarProductoResponseDto
                            {
                                IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                                Codigo = reader.IsDBNull(reader.GetOrdinal("Codigo")) ? null : reader.GetString(reader.GetOrdinal("Codigo")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                                StockDisponible = reader.GetInt32(reader.GetOrdinal("StockDisponible")),
                                Categoria = reader.GetString(reader.GetOrdinal("Categoria"))
                            });
                        }
                    }
                }
            }
            finally
            {
                if (debeCerrar) connection.Close();
            }

            return resultado;
        }
    }
}
