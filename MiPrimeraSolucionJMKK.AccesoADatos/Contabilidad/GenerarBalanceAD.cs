using System;
using System.Collections.Generic;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    public class GenerarBalanceAD : IGenerarBalanceAD
    {
        private readonly Contexto _elContexto;

        public GenerarBalanceAD()
        {
            _elContexto = new Contexto();
        }

        public bool ExistenDatosEnRango(DateTime fechaInicio, DateTime fechaFin)
        {
            bool hayVentas = _elContexto.Database.SqlQuery<int?>(
                @"SELECT TOP 1 1 FROM PUBROCK_VENTA_TB
                  WHERE FECHA_VENTA BETWEEN @p0 AND @p1 AND ID_ESTADO = 1",
                fechaInicio.Date, fechaFin.Date
            ).FirstOrDefault() != null;

            bool hayMovimientos = _elContexto.Database.SqlQuery<int?>(
                @"SELECT TOP 1 1 FROM PUBROCK_MOVIMIENTO_FINANCIERO_TB
                  WHERE FECHA_MOVIMIENTO BETWEEN @p0 AND @p1 AND ID_ESTADO = 1",
                fechaInicio.Date, fechaFin.Date
            ).FirstOrDefault() != null;

            return hayVentas || hayMovimientos;
        }

        public BalanceDto Generar(DateTime fechaInicio, DateTime fechaFin)
        {
            var balance = new BalanceDto
            {
                FechaInicio = fechaInicio.Date,
                FechaFin = fechaFin.Date
            };

            decimal totalVentas = _elContexto.Database.SqlQuery<decimal?>(
                @"SELECT SUM(TOTAL_VENTA) FROM PUBROCK_VENTA_TB
                  WHERE FECHA_VENTA BETWEEN @p0 AND @p1 AND ID_ESTADO = 1",
                fechaInicio.Date, fechaFin.Date
            ).FirstOrDefault() ?? 0m;

            var ingresosPorCategoria = _elContexto.Database.SqlQuery<BalanceCategoriaDto>(
                @"SELECT ISNULL(mf.CATEGORIA,'General') AS Categoria, SUM(mf.MONTO) AS Monto
                  FROM PUBROCK_MOVIMIENTO_FINANCIERO_TB mf
                  INNER JOIN PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB t
                      ON t.ID_TIPO_MOVIMIENTO_FINANCIERO = mf.ID_TIPO_MOVIMIENTO_FINANCIERO
                  WHERE mf.FECHA_MOVIMIENTO BETWEEN @p0 AND @p1
                    AND mf.ID_ESTADO = 1 AND t.DESCRIPCION = 'Ingreso'
                  GROUP BY mf.CATEGORIA",
                fechaInicio.Date, fechaFin.Date
            ).ToList();

            var egresosPorCategoria = _elContexto.Database.SqlQuery<BalanceCategoriaDto>(
                @"SELECT ISNULL(mf.CATEGORIA,'General') AS Categoria, SUM(mf.MONTO) AS Monto
                  FROM PUBROCK_MOVIMIENTO_FINANCIERO_TB mf
                  INNER JOIN PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB t
                      ON t.ID_TIPO_MOVIMIENTO_FINANCIERO = mf.ID_TIPO_MOVIMIENTO_FINANCIERO
                  WHERE mf.FECHA_MOVIMIENTO BETWEEN @p0 AND @p1
                    AND mf.ID_ESTADO = 1 AND t.DESCRIPCION = 'Egreso'
                  GROUP BY mf.CATEGORIA",
                fechaInicio.Date, fechaFin.Date
            ).ToList();

            decimal totalIngresosVariables = ingresosPorCategoria.Sum(i => i.Monto);
            decimal totalEgresos = egresosPorCategoria.Sum(e => e.Monto);

            balance.Desglose.Add(new BalanceCategoriaDto { Tipo = "Ingreso", Categoria = "Ventas", Monto = totalVentas });

            foreach (var ingreso in ingresosPorCategoria)
            {
                ingreso.Tipo = "Ingreso";
                balance.Desglose.Add(ingreso);
            }

            foreach (var egreso in egresosPorCategoria)
            {
                egreso.Tipo = "Egreso";
                balance.Desglose.Add(egreso);
            }

            balance.TotalIngresos = totalVentas + totalIngresosVariables;
            balance.TotalEgresos = totalEgresos;
            balance.UtilidadNeta = balance.TotalIngresos - balance.TotalEgresos;
            balance.EsPerdida = balance.UtilidadNeta < 0;

            return balance;
        }
    }
}
