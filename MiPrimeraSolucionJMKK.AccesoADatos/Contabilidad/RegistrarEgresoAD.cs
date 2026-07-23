using System;
using System.Linq;
using GestionPubRock.AccesoADatos.Common;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    public class RegistrarEgresoAD : IRegistrarEgresoAD
    {
        private readonly Contexto _elContexto;

        public RegistrarEgresoAD()
        {
            _elContexto = new Contexto();
        }

        public int Registrar(EgresoDto egreso)
        {
            try
            {
                int idEmpleado = EmpleadoResolver.ObtenerOCrearIdEmpleado(_elContexto, egreso.RegistradoPor);

                if (idEmpleado <= 0)
                    return -3;

                int idTipoEgreso = _elContexto.Database.SqlQuery<int?>(
                    "SELECT TOP 1 ID_TIPO_MOVIMIENTO_FINANCIERO FROM PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB WHERE DESCRIPCION = 'Egreso'"
                ).FirstOrDefault() ?? 0;

                if (idTipoEgreso <= 0)
                    return -99;

                var movimiento = new MovimientoFinancieroEntidad
                {
                    FechaMovimiento = egreso.Fecha,
                    Descripcion = egreso.Descripcion,
                    Monto = egreso.Monto,
                    IdTipoMovimientoFinanciero = idTipoEgreso,
                    IdEmpleado = idEmpleado,
                    IdEstado = EmpleadoResolver.ObtenerIdEstadoActivo(_elContexto),
                    Categoria = egreso.Categoria,
                    EsFijo = string.Equals(egreso.Categoria, "Servicios", StringComparison.OrdinalIgnoreCase),
                    NumeroComprobante = "EG-" + DateTime.Now.ToString("yyyyMMddHHmmss")
                };

                _elContexto.MovimientosFinancieros.Add(movimiento);

                int filasAfectadas = _elContexto.SaveChanges();

                egreso.IdMovimientoFinanciero = movimiento.IdMovimientoFinanciero;
                egreso.EsFijo = movimiento.EsFijo;
                egreso.NumeroComprobante = movimiento.NumeroComprobante;

                return filasAfectadas > 0 ? movimiento.IdMovimientoFinanciero : -99;
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
