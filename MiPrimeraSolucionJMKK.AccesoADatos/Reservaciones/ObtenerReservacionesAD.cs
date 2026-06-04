using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using GestionPubRock.AccesoADatos;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reservacion;

namespace GestionPubRock.AccesoADatos.Reservaciones
{
    public class ObtenerReservacionesAD
    {
        private Contexto _ctx;

        public ObtenerReservacionesAD()
        {
            _ctx = new Contexto();
        }

        public List<ReservacionDto> Obtener(DateTime? fecha, bool? estado)
        {
            // Ejecutar lector directo y mapear columnas de forma flexible
            var lista = new System.Collections.Generic.List<ReservacionDto>();
            var conn = _ctx.Database.Connection;
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM PUBROCK_RESERVACION_TB WHERE 1=1";
                if (fecha.HasValue)
                {
                    cmd.CommandText += " AND CONVERT(date, FECHA_RESERVACION) = @fecha";
                    var p = cmd.CreateParameter(); p.ParameterName = "@fecha"; p.Value = fecha.Value.Date; cmd.Parameters.Add(p);
                }
                if (estado.HasValue)
                {
                    cmd.CommandText += " AND ISNULL(ID_ESTADO,0) = @estado";
                    var p2 = cmd.CreateParameter(); p2.ParameterName = "@estado"; p2.Value = estado.Value ? 1 : 0; cmd.Parameters.Add(p2);
                }

                if (conn.State != System.Data.ConnectionState.Open) conn.Open();
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        int GetOrdinalIfExists(string[] candidates)
                        {
                            foreach (var c in candidates)
                                try { var ord = rdr.GetOrdinal(c); return ord; } catch { }
                            return -1;
                        }

                        var dto = new ReservacionDto();

                        int ordId = GetOrdinalIfExists(new[] { "ID_RESERVACION", "ID_RESERVACION", "Id", "ID" });
                        if (ordId >= 0 && !rdr.IsDBNull(ordId)) dto.Id = Convert.ToInt32(rdr.GetValue(ordId));

                        int ordFecha = GetOrdinalIfExists(new[] { "FECHA_RESERVACION", "FECHA", "Fecha" });
                        if (ordFecha >= 0 && !rdr.IsDBNull(ordFecha)) dto.Fecha = Convert.ToDateTime(rdr.GetValue(ordFecha));

                        int ordEstado = GetOrdinalIfExists(new[] { "ID_ESTADO", "ESTADO", "IdEstado" });
                        if (ordEstado >= 0 && !rdr.IsDBNull(ordEstado)) dto.Estado = Convert.ToInt32(rdr.GetValue(ordEstado)) == 1;

                        int ordCliente = GetOrdinalIfExists(new[] { "NOMBRE_CLIENTE", "CLIENTE", "CEDULA_CLIENTE", "CLIENTE_NOMBRE" });
                        if (ordCliente >= 0 && !rdr.IsDBNull(ordCliente)) dto.Cliente = rdr.GetValue(ordCliente).ToString();

                        int ordObs = GetOrdinalIfExists(new[] { "OBSERVACIONES", "OBSERVACION", "OBSERV" });
                        if (ordObs >= 0 && !rdr.IsDBNull(ordObs)) dto.Observaciones = rdr.GetValue(ordObs).ToString();

                        lista.Add(dto);
                    }
                }
            }

            return lista;
        }
    }
}
