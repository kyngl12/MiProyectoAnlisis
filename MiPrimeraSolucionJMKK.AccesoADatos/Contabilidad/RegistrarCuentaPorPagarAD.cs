using GestionPubRock.AccesoADatos.Common;
using GestionPubRock.AccesoADatos.Entidades;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    public class RegistrarCuentaPorPagarAD : IRegistrarCuentaPorPagarAD
    {
        private readonly Contexto _elContexto;

        public RegistrarCuentaPorPagarAD()
        {
            _elContexto = new Contexto();
        }

        public int Registrar(CuentaPorPagarDto cuenta)
        {
            try
            {
                var entidad = new CuentaPorPagarEntidad
                {
                    Proveedor = cuenta.Proveedor,
                    Monto = cuenta.Monto,
                    Descripcion = cuenta.Descripcion,
                    FechaRegistro = System.DateTime.Now.Date,
                    FechaVencimiento = cuenta.FechaVencimiento.Date,
                    EstadoPago = "Pendiente",
                    NotificacionEnviada = false,
                    CedulaRegistro = cuenta.RegistradoPor,
                    IdEstado = EmpleadoResolver.ObtenerIdEstadoActivo(_elContexto)
                };

                _elContexto.CuentasPorPagar.Add(entidad);
                int filasAfectadas = _elContexto.SaveChanges();

                cuenta.IdCuentaPorPagar = entidad.IdCuentaPorPagar;

                return filasAfectadas > 0 ? entidad.IdCuentaPorPagar : -99;
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
