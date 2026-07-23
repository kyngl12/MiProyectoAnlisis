using System;
using System.Linq;
using GestionPubRock.AccesoADatos.Common;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    public class MarcarCuentaPorPagarComoPagadaAD : IMarcarCuentaPorPagarComoPagadaAD
    {
        private readonly Contexto _elContexto;

        public MarcarCuentaPorPagarComoPagadaAD()
        {
            _elContexto = new Contexto();
        }

        public int MarcarComoPagada(int idCuentaPorPagar, string cedulaRegistro)
        {
            try
            {
                var cuenta = _elContexto.CuentasPorPagar
                    .FirstOrDefault(c => c.IdCuentaPorPagar == idCuentaPorPagar && c.IdEstado == 1);

                // No existe
                if (cuenta == null)
                    return -1;

                // Ya estaba pagada
                if (cuenta.EstadoPago == "Pagado")
                    return -2;

                int idEmpleado = EmpleadoResolver.ObtenerOCrearIdEmpleado(_elContexto, cedulaRegistro);
                if (idEmpleado <= 0)
                    return -3;

                int idTipoEgreso = _elContexto.Database.SqlQuery<int?>(
                    "SELECT TOP 1 ID_TIPO_MOVIMIENTO_FINANCIERO FROM PUBROCK_TIPO_MOVIMIENTO_FINANCIERO_TB WHERE DESCRIPCION = 'Egreso'"
                ).FirstOrDefault() ?? 0;

                var movimiento = new MovimientoFinancieroEntidad
                {
                    FechaMovimiento = DateTime.Now.Date,
                    Descripcion = "Pago a proveedor: " + cuenta.Proveedor + " - " + cuenta.Descripcion,
                    Monto = cuenta.Monto,
                    IdTipoMovimientoFinanciero = idTipoEgreso,
                    IdEmpleado = idEmpleado,
                    IdEstado = EmpleadoResolver.ObtenerIdEstadoActivo(_elContexto),
                    Categoria = "Cuentas por Pagar",
                    EsFijo = false,
                    NumeroComprobante = "CXP-" + DateTime.Now.ToString("yyyyMMddHHmmss")
                };

                _elContexto.MovimientosFinancieros.Add(movimiento);
                _elContexto.SaveChanges();

                cuenta.EstadoPago = "Pagado";
                cuenta.FechaPago = DateTime.Now.Date;
                cuenta.IdMovimientoFinanciero = movimiento.IdMovimientoFinanciero;

                int filasAfectadas = _elContexto.SaveChanges();

                return filasAfectadas > 0 ? 1 : -99;
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
