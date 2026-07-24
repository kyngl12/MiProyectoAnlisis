using GestionPubRock.AccesoADatos.Common;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    public class RegistrarCierreCajaAD : IRegistrarCierreCajaAD
    {
        private readonly Contexto _elContexto;

        public RegistrarCierreCajaAD()
        {
            _elContexto = new Contexto();
        }

        public int Registrar(CierreCajaDto cierre)
        {
            try
            {
                int idEmpleado = EmpleadoResolver.ObtenerOCrearIdEmpleado(_elContexto, cierre.RegistradoPor);

                if (idEmpleado <= 0)
                    return -3;

                var entidad = new CierreCajaEntidad
                {
                    FechaCierre = cierre.FechaCierre.Date,
                    TotalVentas = cierre.TotalVentas,
                    TotalIngresos = cierre.TotalIngresos,
                    TotalEgresos = cierre.TotalEgresos,
                    BalanceFinal = cierre.BalanceFinal,
                    MontoContado = cierre.MontoContado,
                    IdEmpleado = idEmpleado,
                    IdEstado = EmpleadoResolver.ObtenerIdEstadoActivo(_elContexto),
                    CedulaRegistro = cierre.RegistradoPor
                };

                _elContexto.CierresCaja.Add(entidad);
                int filasAfectadas = _elContexto.SaveChanges();

                cierre.IdCierreCaja = entidad.IdCierreCaja;

                return filasAfectadas > 0 ? entidad.IdCierreCaja : -99;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                // Incluye la violacion del indice unico
                // UQ_PUBROCK_CIERRE_CAJA_FECHA_ACTIVO (doble cierre).
                return -1;
            }
            catch
            {
                throw;
            }
        }
    }
}
