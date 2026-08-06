using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.Reportes;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionPubRock.AccesoADatos.Reportes
{
    public class ReporteVentasAD : IReporteVentasAD
    {
        private readonly Contexto _elContexto;

        public ReporteVentasAD()
        {
            _elContexto = new Contexto();
        }

        public ReporteVentasDto Generar(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                DateTime inicio = fechaInicio.Date;
                DateTime fin = fechaFin.Date;

                var consulta =
                    from venta in _elContexto.Ventas
                    join factura in _elContexto.Facturas
                        on venta.IdVenta equals factura.IdVenta
                    join tipoPago in _elContexto.TiposPago
                        on factura.IdTipoPago equals tipoPago.IdTipoPago
                    where venta.FechaVenta >= inicio
                       && venta.FechaVenta <= fin
                       && venta.IdEstado == 1
                       && factura.IdEstado == 1
                    select new
                    {
                        Venta = venta,
                        Factura = factura,
                        TipoPago = tipoPago
                    };

                var registros = consulta.ToList();

                var reporte = new ReporteVentasDto
                {
                    FechaInicio = inicio,
                    FechaFin = fin,
                    CantidadTransacciones = registros.Count,
                    MontoFinalRecaudado = registros.Sum(x => x.Factura.MontoTotal),

                    Ventas = registros.Select(x => new DetalleVentaReporteDto
                    {
                        IdVenta = x.Venta.IdVenta,
                        FechaVenta = x.Venta.FechaVenta,
                        NumeroFactura = x.Factura.NumeroFactura,
                        MetodoPago = x.TipoPago.Descripcion,
                        TotalVenta = x.Factura.MontoTotal
                    }).ToList(),

                    MetodosPago = registros
                        .GroupBy(x => x.TipoPago.Descripcion)
                        .Select(grupo => new MetodoPagoReporteDto
                        {
                            MetodoPago = grupo.Key,
                            CantidadTransacciones = grupo.Count(),
                            MontoTotal = grupo.Sum(x => x.Factura.MontoTotal)
                        })
                        .ToList()
                };

                return reporte;
            }
            catch
            {
                throw;
            }
        }
    }
}
