using System;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reservacion;

namespace GestionPubRock.AccesoADatos.Reservaciones
{
    public class RegistrarReservacionAD
    {
        private readonly Contexto _ctx;

        public RegistrarReservacionAD()
        {
            _ctx = new Contexto();
        }

        public int Registrar(ReservacionRequestDto dto)
        {
            try
            {
                // Validar existencia cliente
                var existeCliente = _ctx.Database.SqlQuery<int?>(
                    "SELECT 1 FROM PUBROCK_CLIENTE_TB WHERE ID_CLIENTE = @p0",
                    dto.IdCliente
                ).FirstOrDefault();

                if (existeCliente == null) return -2; // cliente no existe

                // Validar capacidad de la mesa
                var capacidad = _ctx.Database.SqlQuery<int?>(
                    "SELECT CAPACIDAD FROM PUBROCK_MESA_TB WHERE ID_MESA = @p0",
                    dto.IdMesa
                ).FirstOrDefault();

                if (capacidad == null) return -3; // mesa no existe
                if (dto.CantidadPersonas > capacidad.Value) return -4; // excede capacidad

                // Validar disponibilidad (no solapamiento)
                var overlapCount = _ctx.Database.SqlQuery<int>(
                    @"SELECT COUNT(1) FROM PUBROCK_RESERVACION_TB
                      WHERE ID_MESA = @p0
                        AND CONVERT(date, FECHA_RESERVACION) = CONVERT(date, @p1)
                        AND NOT (HORA_FIN <= @p2 OR HORA_INICIO >= @p3)",
                    dto.IdMesa,
                    dto.Fecha.Date,
                    dto.HoraInicio,
                    dto.HoraFin
                ).FirstOrDefault();

                if (overlapCount > 0) return -5; // mesa no disponible

                // Evitar reservaciones duplicadas del mismo cliente en la misma fecha y horario
                var duplicateClientCount = _ctx.Database.SqlQuery<int>(
                    @"SELECT COUNT(1) FROM PUBROCK_RESERVACION_TB
                      WHERE ID_CLIENTE = @p0
                        AND CONVERT(date, FECHA_RESERVACION) = CONVERT(date, @p1)
                        AND NOT (HORA_FIN <= @p2 OR HORA_INICIO >= @p3)",
                    dto.IdCliente,
                    dto.Fecha.Date,
                    dto.HoraInicio,
                    dto.HoraFin
                ).FirstOrDefault();

                if (duplicateClientCount > 0) return -6; // cliente ya tiene reservación en ese horario

                // Insertar reservacion con estado inicial = 'Activo' (obtenido de PUBROCK_ESTADO_TB)
                var idEstadoActivo = _ctx.Database.SqlQuery<int?>("SELECT TOP 1 ID_ESTADO FROM PUBROCK_ESTADO_TB WHERE DESCRIPCION = 'Activo'").FirstOrDefault() ?? 1;

                var result = _ctx.Database.ExecuteSqlCommand(
                    @"INSERT INTO PUBROCK_RESERVACION_TB
                      (FECHA_RESERVACION, HORA_INICIO, HORA_FIN, CANTIDAD_PERSONAS, OBSERVACIONES, ID_CLIENTE, ID_MESA, ID_ESTADO)
                      VALUES(@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7)",
                    dto.Fecha.Date,
                    dto.HoraInicio,
                    dto.HoraFin,
                    dto.CantidadPersonas,
                    (object)dto.Observaciones ?? DBNull.Value,
                    dto.IdCliente,
                    dto.IdMesa,
                    idEstadoActivo
                );

                return result; // normalmente 1 si insertó
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return -99;
            }
            catch
            {
                throw;
            }
        }
    }
}
