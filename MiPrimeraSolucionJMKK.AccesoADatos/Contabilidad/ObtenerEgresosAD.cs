using System;
using System.Collections.Generic;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    public class ObtenerEgresosAD : IObtenerEgresosAD
    {
        private readonly Contexto _elContexto;

        public ObtenerEgresosAD()
        {
            _elContexto = new Contexto();
        }

        public List<EgresoDto> Obtener(DateTime? fechaInicio, DateTime? fechaFin)
        {
            var consulta =
                from mf in _elContexto.MovimientosFinancieros
                join tipo in _elContexto.TiposMovimientoFinanciero
                    on mf.IdTipoMovimientoFinanciero equals tipo.IdTipoMovimientoFinanciero
                where tipo.Descripcion == "Egreso" && mf.IdEstado == 1
                select new EgresoDto
                {
                    IdMovimientoFinanciero = mf.IdMovimientoFinanciero,
                    Monto = mf.Monto,
                    Descripcion = mf.Descripcion,
                    Fecha = mf.FechaMovimiento
                };

            if (fechaInicio.HasValue)
                consulta = consulta.Where(dto => dto.Fecha >= fechaInicio.Value.Date);

            if (fechaFin.HasValue)
                consulta = consulta.Where(dto => dto.Fecha <= fechaFin.Value.Date);

            return consulta
                .OrderByDescending(dto => dto.Fecha)
                .ToList();
        }
    }
}
