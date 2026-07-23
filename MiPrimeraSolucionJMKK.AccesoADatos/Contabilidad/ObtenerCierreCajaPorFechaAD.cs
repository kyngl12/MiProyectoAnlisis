using System;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    public class ObtenerCierreCajaPorFechaAD : IObtenerCierreCajaPorFechaAD
    {
        private readonly Contexto _elContexto;

        public ObtenerCierreCajaPorFechaAD()
        {
            _elContexto = new Contexto();
        }

        public CierreCajaDto ObtenerPorFecha(DateTime fecha)
        {
            var cierre = _elContexto.CierresCaja
                .Where(c => c.FechaCierre == fecha.Date && c.IdEstado == 1)
                .OrderByDescending(c => c.IdCierreCaja)
                .FirstOrDefault();

            if (cierre == null)
                return null;

            return new CierreCajaDto
            {
                IdCierreCaja = cierre.IdCierreCaja,
                FechaCierre = cierre.FechaCierre,
                TotalVentas = cierre.TotalVentas,
                TotalIngresos = cierre.TotalIngresos,
                TotalEgresos = cierre.TotalEgresos,
                BalanceFinal = cierre.BalanceFinal,
                MontoContado = cierre.MontoContado,
                Diferencia = cierre.MontoContado - cierre.BalanceFinal,
                RegistradoPor = cierre.CedulaRegistro
            };
        }
    }
}
