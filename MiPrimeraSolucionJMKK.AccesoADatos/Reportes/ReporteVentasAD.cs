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

        public ReporteVentasDto GenerarFiltrado(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado)
        {
            try
            {
                DateTime inicio = (fechaInicio ?? DateTime.MinValue).Date;
                DateTime fin = (fechaFin ?? DateTime.MaxValue).Date;

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

                // Filtrar por tipo de pago
                if (idTipoPago.HasValue)
                {
                    consulta = consulta.Where(x => x.Factura.IdTipoPago == idTipoPago.Value);
                }

                // Filtrar por cliente
                if (idCliente.HasValue)
                {
                    consulta = consulta.Where(x => x.Venta.IdCliente.HasValue && x.Venta.IdCliente.Value == idCliente.Value);
                }

                // Filtrar por estado de venta
                if (idEstado.HasValue)
                {
                    consulta = consulta.Where(x => x.Venta.IdEstado == idEstado.Value);
                }

                var registros = consulta.ToList();

                // Filtrar por categoría requiere inspeccionar detalle de venta y productos
                List<int> ventasConCategoria = null;
                if (idCategoria.HasValue)
                {
                    var sql = @"SELECT DISTINCT ID_VENTA FROM PUBROCK_DETALLE_VENTA_TB dv
                                INNER JOIN PUBROCK_PRODUCTO_TB p ON dv.ID_PRODUCTO = p.ID_PRODUCTO
                                WHERE p.ID_CATEGORIA_PRODUCTO = @p0 AND dv.ID_ESTADO = 1";

                    ventasConCategoria = _elContexto.Database.SqlQuery<int>(sql, idCategoria.Value).ToList();

                    registros = registros.Where(r => ventasConCategoria.Contains(r.Venta.IdVenta)).ToList();
                }

                var reporte = new ReporteVentasDto
                {
                    FechaInicio = fechaInicio?.Date ?? inicio,
                    FechaFin = fechaFin?.Date ?? fin,
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
