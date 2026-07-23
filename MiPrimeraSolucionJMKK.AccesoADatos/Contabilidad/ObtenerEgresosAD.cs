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
                select mf;

            if (fechaInicio.HasValue)
                consulta = consulta.Where(mf => mf.FechaMovimiento >= fechaInicio.Value.Date);

            if (fechaFin.HasValue)
                consulta = consulta.Where(mf => mf.FechaMovimiento <= fechaFin.Value.Date);

            return consulta
                .OrderByDescending(mf => mf.FechaMovimiento)
                .ToList()
                .Select(mf => new EgresoDto
                {
                    IdMovimientoFinanciero = mf.IdMovimientoFinanciero,
                    Monto = mf.Monto,
                    Descripcion = mf.Descripcion,
                    Categoria = mf.Categoria,
                    Fecha = mf.FechaMovimiento,
                    EsFijo = mf.EsFijo,
                    NumeroComprobante = mf.NumeroComprobante
                })
                .ToList();
        }
    }
}
