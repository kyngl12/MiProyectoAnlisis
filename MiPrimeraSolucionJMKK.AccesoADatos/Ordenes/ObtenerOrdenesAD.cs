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
                SELECT o.ID_ORDEN AS IdOrden, o.FECHA_ORDEN AS FechaOrden, o.OBSERVACIONES AS Observaciones,
                       (SELECT COUNT(1) FROM PUBROCK_DETALLE_ORDEN_TB d WHERE d.ID_ORDEN = o.ID_ORDEN) AS CantidadItems,
                       o.ID_ESTADO_ORDEN AS IdEstadoOrden, eo.DESCRIPCION AS EstadoOrden
                FROM PUBROCK_ORDEN_TB o
                LEFT JOIN PUBROCK_ESTADO_ORDEN_TB eo ON o.ID_ESTADO_ORDEN = eo.ID_ESTADO_ORDEN
                ORDER BY o.ID_ORDEN DESC
            ";

            var raw = _ctx.Database.SqlQuery<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ordenes.OrdenListDto>(sql);
            var list = new List<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ordenes.OrdenListDto>();
            foreach (var item in raw)
                list.Add(item);
            return list;
        }
    }
}
