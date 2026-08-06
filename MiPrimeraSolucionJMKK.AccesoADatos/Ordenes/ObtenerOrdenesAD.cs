using System.Collections.Generic;
using System.Linq;
using GestionPubRock.AccesoADatos;

namespace GestionPubRock.AccesoADatos.Ordenes
{
    public class ObtenerOrdenesAD
    {
        private readonly Contexto _ctx;

        public ObtenerOrdenesAD()
        {
            _ctx = new Contexto();
        }

        public List<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ordenes.OrdenListDto> ObtenerTodos()
        {
            string sql = @"
    SELECT
        o.ID_ORDEN AS IdOrden,
        o.FECHA_ORDEN AS FechaOrden,
        o.OBSERVACIONES AS Observaciones,
        (
            SELECT COUNT(1)
            FROM PUBROCK_DETALLE_ORDEN_TB d
            WHERE d.ID_ORDEN = o.ID_ORDEN
        ) AS CantidadItems,
        o.ID_ESTADO_ORDEN AS IdEstadoOrden,
        eo.DESCRIPCION AS EstadoOrdenDescripcion
    FROM PUBROCK_ORDEN_TB o
    LEFT JOIN PUBROCK_ESTADO_ORDEN_TB eo
        ON o.ID_ESTADO_ORDEN = eo.ID_ESTADO_ORDEN
    WHERE o.ID_ESTADO = (
        SELECT TOP 1 ID_ESTADO
        FROM PUBROCK_ESTADO_TB
        WHERE DESCRIPCION = 'Activo'
        ORDER BY ID_ESTADO
    )
    ORDER BY o.ID_ORDEN DESC
";

            return _ctx.Database
    .SqlQuery<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ordenes.OrdenListDto>(sql)
    .ToList();
        }
    }
}
