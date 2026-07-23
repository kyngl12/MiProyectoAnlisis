using System;
using System.Collections.Generic;
using System.Linq;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;

namespace GestionPubRock.AccesoADatos.Contabilidad
{
    public class ObtenerCuentasPorPagarAD : IObtenerCuentasPorPagarAD
    {
        private readonly Contexto _elContexto;

        public ObtenerCuentasPorPagarAD()
        {
            _elContexto = new Contexto();
        }

        public List<CuentaPorPagarDto> Obtener()
        {
            DateTime hoy = DateTime.Now.Date;

            return _elContexto.CuentasPorPagar
                .Where(c => c.IdEstado == 1)
                .OrderBy(c => c.FechaVencimiento)
                .ToList()
                .Select(c => new CuentaPorPagarDto
                {
                    IdCuentaPorPagar = c.IdCuentaPorPagar,
                    Proveedor = c.Proveedor,
                    Monto = c.Monto,
                    Descripcion = c.Descripcion,
                    FechaRegistro = c.FechaRegistro,
                    FechaVencimiento = c.FechaVencimiento,
                    FechaPago = c.FechaPago,
                    EstadoPago = c.EstadoPago,
                    EstaVencida = c.EstadoPago == "Pendiente" && c.FechaVencimiento < hoy,
                    DiasParaVencer = (c.FechaVencimiento.Date - hoy).Days,
                    RegistradoPor = c.CedulaRegistro
                })
                .ToList();
        }
    }
}
