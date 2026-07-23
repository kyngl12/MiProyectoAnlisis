using System;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    public class ObtenerResumenDelDiaAD : IObtenerResumenDelDiaAD
    {
        private readonly Contexto _elContexto;

        public ObtenerResumenDelDiaAD()
        {
            _elContexto = new Contexto();
        }

        public CierreCajaDto ObtenerResumen(DateTime fecha)
        {
            DateTime laFecha = fecha.Date;

            decimal totalVentas = _elContexto.Database.SqlQuery<decimal?>(
                @"SELECT SUM(TOTAL_VENTA) FROM PUBROCK_VENTA_TB
                  WHERE FECHA_VENTA = @p0 AND ID_ESTADO = 1",
                laFecha
            ).FirstOrDefault() ?? 0m;

            decimal totalIngresos = _elContexto.Database.SqlQuery<decimal?>(
                @"SELECT SUM(mf.MONTO)
                  FROM PUBROCK_MOVIMIENTO_FINANCIERO_TB mf
                  INNER JOIN PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB t
                      ON t.ID_TIPO_MOVIMIENTO_FINANCIERO = mf.ID_TIPO_MOVIMIENTO_FINANCIERO
                  WHERE mf.FECHA_MOVIMIENTO = @p0 AND mf.ID_ESTADO = 1 AND t.DESCRIPCION = 'Ingreso'",
                laFecha
            ).FirstOrDefault() ?? 0m;

            decimal totalEgresos = _elContexto.Database.SqlQuery<decimal?>(
                @"SELECT SUM(mf.MONTO)
                  FROM PUBROCK_MOVIMIENTO_FINANCIERO_TB mf
                  INNER JOIN PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB t
                      ON t.ID_TIPO_MOVIMIENTO_FINANCIERO = mf.ID_TIPO_MOVIMIENTO_FINANCIERO
                  WHERE mf.FECHA_MOVIMIENTO = @p0 AND mf.ID_ESTADO = 1 AND t.DESCRIPCION = 'Egreso'",
                laFecha
            ).FirstOrDefault() ?? 0m;

            return new CierreCajaDto
            {
                FechaCierre = laFecha,
                TotalVentas = totalVentas,
                TotalIngresos = totalIngresos,
                TotalEgresos = totalEgresos,
                BalanceFinal = totalVentas + totalIngresos - totalEgresos
            };
        }
    }
}
