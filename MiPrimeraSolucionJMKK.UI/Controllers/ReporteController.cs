using GestionPubRock.LogicaDeNegocio.Reportes;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Reportes;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
#if EXPORT_REPORTS
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
#endif
using System.Data;
using System.Data.SqlClient;

namespace GestionPubRock.UI.Controllers
{
    
    [Authorize(Roles = "Administrador")]
    public class ReporteController : Controller
    {
        private readonly IReporteVentasLN _reporteVentasLN;

        private void CargarFiltros()
        {
            try
            {
                using (var ctx = new GestionPubRock.AccesoADatos.Contexto())
                {
                    var categorias = ctx.Database.SqlQuery<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes.CategoriaDto>("SELECT ID_CATEGORIA_PRODUCTO AS Id, NOMBRE_CATEGORIA AS Nombre FROM PUBROCK_CATEGORIA_PRODUCTO_TB WHERE ID_ESTADO = 1").ToList();
                    ViewBag.Categorias = categorias;

                    var tiposPago = ctx.TiposPago.Where(t => t.IdEstado == 1).Select(t => new MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes.TipoPagoDto { Id = t.IdTipoPago, Nombre = t.Descripcion }).ToList();
                    ViewBag.TiposPago = tiposPago;

                    var estados = ctx.Database.SqlQuery<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes.EstadoDto>("SELECT ID_ESTADO AS Id, DESCRIPCION AS Nombre FROM PUBROCK_ESTADO_TB").ToList();
                    ViewBag.Estados = estados;
                }
            }
            catch
            {
                ViewBag.Categorias = new List<object>();
                ViewBag.TiposPago = new List<object>();
                ViewBag.Estados = new List<object>();
            }
        }

        public ReporteController()
        {
            _reporteVentasLN = new ReporteVentasLN();
        }

#if EXPORT_REPORTS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportarExcel(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado)
        {
            try
            {
                using (var ctx = new GestionPubRock.AccesoADatos.Contexto())
                {
                    var p1 = new SqlParameter("@FechaInicio", (object)fechaInicio ?? DBNull.Value);
                    var p2 = new SqlParameter("@FechaFin", (object)fechaFin ?? DBNull.Value);
                    var p3 = new SqlParameter("@IdCategoria", (object)idCategoria ?? DBNull.Value);
                    var p4 = new SqlParameter("@IdTipoPago", (object)idTipoPago ?? DBNull.Value);
                    var p5 = new SqlParameter("@IdCliente", (object)idCliente ?? DBNull.Value);
                    var p6 = new SqlParameter("@IdEstado", (object)idEstado ?? DBNull.Value);

                    var rows = ctx.Database.SqlQuery<ReporteExportRowDto>(
                        "EXEC SP_PUBROCK_REPORTE_VENTAS @FechaInicio, @FechaFin, @IdCategoria, @IdTipoPago, @IdCliente, @IdEstado",
                        p1, p2, p3, p4, p5, p6).ToList();

                    using (var ms = new MemoryStream())
                    using (var workbook = new XLWorkbook())
                    {
                        var ws = workbook.Worksheets.Add("ReporteVentas");

                        // Encabezado institucional
                        ws.Cell(1, 1).Value = "Nombre del negocio";
                        ws.Cell(2, 1).Value = "Reporte de ventas";
                        ws.Cell(3, 1).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        // Cabeceras
                        var headerRow = 5;
                        var headers = new[] { "Número factura", "Fecha", "Cliente", "Usuario", "Método de pago", "Subtotal", "IVA", "Descuento", "Total", "Estado" };
                        for (int i = 0; i < headers.Length; i++)
                        {
                            ws.Cell(headerRow, i + 1).Value = headers[i];
                            ws.Cell(headerRow, i + 1).Style.Font.Bold = true;
                        }

                        var r = headerRow + 1;
                        foreach (var row in rows)
                        {
                            ws.Cell(r, 1).Value = row.NumeroFactura;
                            ws.Cell(r, 2).Value = row.Fecha;
                            ws.Cell(r, 3).Value = row.Cliente;
                            ws.Cell(r, 4).Value = row.Usuario;
                            ws.Cell(r, 5).Value = row.MetodoPago;
                            ws.Cell(r, 6).Value = row.Subtotal;
                            ws.Cell(r, 7).Value = row.IVA;
                            ws.Cell(r, 8).Value = row.Descuento;
                            ws.Cell(r, 9).Value = row.Total;
                            ws.Cell(r, 10).Value = row.Estado;
                            r++;
                        }

                        ws.Columns().AdjustToContents();
                        workbook.SaveAs(ms);
                        var content = ms.ToArray();
                        var fileName = $"ReporteVentas_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }

        [HttpGet]
        public ActionResult ReporteVentasJson(int page = 1, int pageSize = 10, string search = null, string sortColumn = "FechaVenta", string sortDir = "desc", DateTime? fechaInicio = null, DateTime? fechaFin = null, int? idCategoria = null, int? idTipoPago = null, int? idCliente = null, int? idEstado = null)
        {
            try
            {
                // Obtener datos filtrados
                var reporte = _reporteVentasLN.GenerarFiltrado(fechaInicio, fechaFin, idCategoria, idTipoPago, idCliente, idEstado);

                var rows = reporte.Ventas.AsQueryable();

                // búsqueda rápida
                if (!string.IsNullOrEmpty(search))
                {
                    var q = search.ToLower();
                    rows = rows.Where(r => r.NumeroFactura.ToLower().Contains(q) || r.MetodoPago.ToLower().Contains(q));
                }

                // ordenamiento simple
                if (sortColumn == "TotalVenta")
                {
                    rows = sortDir == "asc" ? rows.OrderBy(r => r.TotalVenta) : rows.OrderByDescending(r => r.TotalVenta);
                }
                else if (sortColumn == "FechaVenta")
                {
                    rows = sortDir == "asc" ? rows.OrderBy(r => r.FechaVenta) : rows.OrderByDescending(r => r.FechaVenta);
                }

                var total = rows.Count();
                var paged = rows.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return Json(new { total, data = paged }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult DashboardData()
        {
            try
            {
                using (var ctx = new GestionPubRock.AccesoADatos.Contexto())
                {
                    var today = DateTime.Today;
                    var endToday = today.AddDays(1);
                    var firstOfMonth = new DateTime(today.Year, today.Month, 1);
                    var endOfMonth = firstOfMonth.AddMonths(1);

                    // Totales: preferir facturas con estado activo; si MontoTotal es 0 o nulo, calcular desde detalle de ventas
                    var totalToday = ctx.Facturas.Where(f => f.FechaFactura >= today && f.FechaFactura < endToday && f.IdEstado == 1).Sum(f => (decimal?)f.MontoTotal) ?? 0m;
                    var totalMonth = ctx.Facturas.Where(f => f.FechaFactura >= firstOfMonth && f.FechaFactura < endOfMonth && f.IdEstado == 1).Sum(f => (decimal?)f.MontoTotal) ?? 0m;

                    if (totalToday == 0m)
                    {
                        var p1 = new SqlParameter("@start", today);
                        var p2 = new SqlParameter("@end", endToday);
                        var sql = "SELECT ISNULL(SUM(dv.SUBTOTAL),0) FROM PUBROCK_DETALLE_VENTA_TB dv INNER JOIN PUBROCK_FACTURA_TB f ON dv.ID_VENTA = f.ID_VENTA WHERE f.FECHA_FACTURA >= @start AND f.FECHA_FACTURA < @end AND dv.ID_ESTADO = 1";
                        totalToday = ctx.Database.SqlQuery<decimal?>(sql, p1, p2).FirstOrDefault() ?? 0m;
                    }

                    if (totalMonth == 0m)
                    {
                        var p3 = new SqlParameter("@startm", firstOfMonth);
                        var p4 = new SqlParameter("@endm", endOfMonth);
                        var sqlm = "SELECT ISNULL(SUM(dv.SUBTOTAL),0) FROM PUBROCK_DETALLE_VENTA_TB dv INNER JOIN PUBROCK_FACTURA_TB f ON dv.ID_VENTA = f.ID_VENTA WHERE f.FECHA_FACTURA >= @startm AND f.FECHA_FACTURA < @endm AND dv.ID_ESTADO = 1";
                        totalMonth = ctx.Database.SqlQuery<decimal?>(sqlm, p3, p4).FirstOrDefault() ?? 0m;
                    }

                    var ventasPorProducto = ctx.Database.SqlQuery<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes.VentaProductoDto>("SELECT p.NOMBRE_PRODUCTO AS Nombre, SUM(dv.CANTIDAD) AS Cant FROM PUBROCK_DETALLE_VENTA_TB dv JOIN PUBROCK_PRODUCTO_TB p ON dv.ID_PRODUCTO = p.ID_PRODUCTO WHERE dv.ID_ESTADO = 1 GROUP BY p.NOMBRE_PRODUCTO ORDER BY Cant DESC").ToList();

                    // Producto más vendido
                    var prodMasVendido = (ventasPorProducto != null && ventasPorProducto.Count > 0) ? ventasPorProducto[0].Nombre : string.Empty;

                    // Cliente con más compras
                    var clienteTop = ctx.Database.SqlQuery<dynamic>("SELECT TOP 1 v.ID_CLIENTE, COUNT(*) AS Cant FROM PUBROCK_VENTA_TB v WHERE v.ID_ESTADO = 1 GROUP BY v.ID_CLIENTE ORDER BY Cant DESC").ToList();

                    // Metodo de pago más usado
                    var metodoTop = (from f in ctx.Facturas where f.IdEstado == 1 group f by f.IdTipoPago into g select new { IdTipoPago = g.Key, Cant = g.Count() }).OrderByDescending(x => x.Cant).FirstOrDefault();
                    var metodoDesc = string.Empty;
                    if (metodoTop != null)
                    {
                        var tp = ctx.TiposPago.Find(metodoTop.IdTipoPago);
                        metodoDesc = tp?.Descripcion ?? string.Empty;
                    }

                    // Ventas por día últimos 30 días
                    var desde = today.AddDays(-29);
                    var ventasDias = ctx.Facturas.Where(f => f.FechaFactura >= desde && f.FechaFactura <= today && f.IdEstado == 1)
                        .GroupBy(f => DbFunctions.TruncateTime(f.FechaFactura))
                        .Select(g => new { Fecha = g.Key.Value, Total = g.Sum(x => x.MontoTotal) })
                        .ToList();

                    // si no hay ventas por día con estado, intentar calcular desde detalle de ventas agrupado por fecha
                    if ((ventasDias == null || ventasDias.Count == 0))
                    {
                        var pStart = new SqlParameter("@pStart", desde);
                        var pEnd = new SqlParameter("@pEnd", endToday);
                        var sqlDays = @"SELECT CONVERT(date, f.FECHA_FACTURA) AS Fecha, SUM(dv.SUBTOTAL) AS Total
                                        FROM PUBROCK_DETALLE_VENTA_TB dv
                                        INNER JOIN PUBROCK_FACTURA_TB f ON dv.ID_VENTA = f.ID_VENTA
                                        WHERE f.FECHA_FACTURA >= @pStart AND f.FECHA_FACTURA < @pEnd AND dv.ID_ESTADO = 1
                                        GROUP BY CONVERT(date, f.FECHA_FACTURA)
                                        ORDER BY Fecha";
                        var dayRows = ctx.Database.SqlQuery<dynamic>(sqlDays, pStart, pEnd).ToList();
                        ventasDias = dayRows.Select(dr => new { Fecha = (DateTime)dr.Fecha, Total = (decimal)dr.Total }).ToList();
                    }

                    // fallback si no hay datos (intentar sin filtrar por estado)
                    if ((ventasDias == null || ventasDias.Count == 0) && (totalToday == 0 && totalMonth == 0))
                    {
                        totalToday = ctx.Facturas.Where(f => f.FechaFactura >= today && f.FechaFactura < endToday).Sum(f => (decimal?)f.MontoTotal) ?? 0m;
                        totalMonth = ctx.Facturas.Where(f => f.FechaFactura >= firstOfMonth && f.FechaFactura < endOfMonth).Sum(f => (decimal?)f.MontoTotal) ?? 0m;
                        ventasDias = ctx.Facturas.Where(f => f.FechaFactura >= desde && f.FechaFactura <= today)
                            .GroupBy(f => DbFunctions.TruncateTime(f.FechaFactura))
                            .Select(g => new { Fecha = g.Key.Value, Total = g.Sum(x => x.MontoTotal) })
                            .ToList();
                    }

                    return Json(new
                    {
                        totalTodayWithState = totalToday,
                        totalMonthWithState = totalMonth,
                        totalToday,
                        totalMonth,
                        prodMasVendido,
                        metodoMasUsado = metodoDesc,
                        ventasPorDia = ventasDias.Select(v => new { fecha = v.Fecha.ToString("yyyy-MM-dd"), total = v.Total }),
                        ventasPorDiaCount = (ventasDias != null) ? ventasDias.Count : 0,
                    ventasPorProducto = ventasPorProducto.Select(v => new { nombre = v.Nombre, cantidad = v.Cant }).Take(10)
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Reporte/FacturasRecent
        [HttpGet]
        public ActionResult FacturasRecent(int top = 50)
        {
            try
            {
                using (var ctx = new GestionPubRock.AccesoADatos.Contexto())
                {
                    var sql = "SELECT TOP (@p0) ID_FACTURA AS IdFactura, FECHA_FACTURA AS FechaFactura, MONTO_TOTAL AS MontoTotal, ID_TIPO_PAGO AS IdTipoPago, ID_ESTADO AS IdEstado, ID_VENTA AS IdVenta FROM PUBROCK_FACTURA_TB ORDER BY FECHA_FACTURA DESC";
                    var param = new SqlParameter("@p0", top);
                    var rows = ctx.Database.SqlQuery<dynamic>(sql, param).ToList();
                    return Json(new { count = rows.Count, rows = rows }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return RedirectToAction("ReporteVentas");
            }
        }
#endif

#if EXPORT_REPORTS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportarPdf(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado)
        {
            try
            {
                using (var ctx = new GestionPubRock.AccesoADatos.Contexto())
                {
                    var p1 = new SqlParameter("@FechaInicio", (object)fechaInicio ?? DBNull.Value);
                    var p2 = new SqlParameter("@FechaFin", (object)fechaFin ?? DBNull.Value);
                    var p3 = new SqlParameter("@IdCategoria", (object)idCategoria ?? DBNull.Value);
                    var p4 = new SqlParameter("@IdTipoPago", (object)idTipoPago ?? DBNull.Value);
                    var p5 = new SqlParameter("@IdCliente", (object)idCliente ?? DBNull.Value);
                    var p6 = new SqlParameter("@IdEstado", (object)idEstado ?? DBNull.Value);

                    var rows = ctx.Database.SqlQuery<ReporteExportRowDto>(
                        "EXEC SP_PUBROCK_REPORTE_VENTAS @FechaInicio, @FechaFin, @IdCategoria, @IdTipoPago, @IdCliente, @IdEstado",
                        p1, p2, p3, p4, p5, p6).ToList();

                    using (var ms = new MemoryStream())
                    {
                        var writer = new PdfWriter(ms);
                        var pdf = new PdfDocument(writer);
                        var doc = new Document(pdf);

                        doc.Add(new Paragraph("Nombre del negocio").SetBold());
                        doc.Add(new Paragraph($"Reporte de ventas - {DateTime.Now:yyyy-MM-dd HH:mm:ss}"));
                        doc.Add(new Paragraph(" "));

                        var table = new Table(UnitValue.CreatePercentArray(new float[] { 2, 2, 3, 3, 2, 2, 2, 2, 2, 2 })).UseAllAvailableWidth();
                        var headers = new[] { "Número factura", "Fecha", "Cliente", "Usuario", "Método de pago", "Subtotal", "IVA", "Descuento", "Total", "Estado" };
                        foreach (var h in headers) table.AddHeaderCell(new Cell().Add(new Paragraph(h)));

                        foreach (var row in rows)
                        {
                            table.AddCell(row.NumeroFactura ?? "");
                            table.AddCell(row.Fecha.ToString("yyyy-MM-dd"));
                            table.AddCell(row.Cliente ?? "");
                            table.AddCell(row.Usuario ?? "");
                            table.AddCell(row.MetodoPago ?? "");
                            table.AddCell(row.Subtotal.ToString("F2"));
                            table.AddCell(row.IVA.ToString("F2"));
                            table.AddCell(row.Descuento.ToString("F2"));
                            table.AddCell(row.Total.ToString("F2"));
                            table.AddCell(row.Estado ?? "");
                        }

                        doc.Add(table);
                        doc.Close();

                        var content = ms.ToArray();
                        var fileName = $"ReporteVentas_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                        return File(content, "application/pdf", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return RedirectToAction("ReporteVentas");
            }
        }
#endif

        // GET: Reporte/ReporteVentas
        public ActionResult ReporteVentas()
        {
            var modelo = new ReporteVentasDto
            {
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today
            };
            // Cargar datos para filtros
            CargarFiltros();

            return View(modelo);
        }

        // GET: Reporte/Dashboard
        public ActionResult Dashboard()
        {
            return View();
        }

        // GET: Reporte/AdminDiagnostics
        // Devuelve información útil para diagnosticar permisos y estado de objetos DB
        [HttpGet]
        public ActionResult AdminDiagnostics()
        {
            try
            {
                var identity = User?.Identity;
                var name = identity?.Name ?? string.Empty;
                var isAuth = identity?.IsAuthenticated ?? false;

                var rolesToCheck = new[] { "Administrador", "Empleado", "Cliente", "Cajero" };
                var roles = rolesToCheck.Select(r => new { Role = r, IsInRole = User.IsInRole(r) }).ToList();

                using (var ctx = new GestionPubRock.AccesoADatos.Contexto())
                {
                    var sysUser = ctx.Database.SqlQuery<dynamic>("SELECT TOP 1 * FROM PUBROCK_USUARIO_SISTEMA_TB WHERE NOMBRE_USUARIO = @p0", name).FirstOrDefault();

                    var spVentasExists = ctx.Database.SqlQuery<int>("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'SP_PUBROCK_REPORTE_VENTAS') AND type = 'P'").FirstOrDefault();
                    var spInvExists = ctx.Database.SqlQuery<int>("SELECT COUNT(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'SP_PUBROCK_REPORTE_INVENTARIO') AND type = 'P'").FirstOrDefault();

                    var facturasCount = ctx.Facturas.Count();

                    return Json(new
                    {
                        userName = name,
                        isAuthenticated = isAuth,
                        roles = roles,
                        sysUser = sysUser,
                        spReporteVentasExists = spVentasExists > 0,
                        spReporteInventarioExists = spInvExists > 0,
                        facturasCount = facturasCount
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Reporte/ReporteVentas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            var modelo = new ReporteVentasDto
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };

            try
            {
                ReporteVentasDto reporte = _reporteVentasLN.Generar(fechaInicio, fechaFin);

                TempData["MensajeExito"] = "Reporte generado correctamente.";

                // asegurar filtros disponibles en la vista
                CargarFiltros();

                return View(reporte);
            }
            catch (ArgumentException ex)
            {
                ViewBag.MensajeError = ex.Message;
                CargarFiltros();
                return View(modelo);
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.MensajeInfo = ex.Message;
                CargarFiltros();
                return View(modelo);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                CargarFiltros();
                return View(modelo);
            }
        }

        // POST: Reporte/ReporteVentasFiltrado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReporteVentasFiltrado(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado)
        {
            var modelo = new ReporteVentasDto
            {
                FechaInicio = fechaInicio ?? DateTime.Today,
                FechaFin = fechaFin ?? DateTime.Today
            };

            try
            {
                ReporteVentasDto reporte = _reporteVentasLN.GenerarFiltrado(fechaInicio, fechaFin, idCategoria, idTipoPago, idCliente, idEstado);

                TempData["MensajeExito"] = "Reporte generado correctamente.";
                CargarFiltros();
                return View("ReporteVentas", reporte);
            }
            catch (ArgumentException ex)
            {
                ViewBag.MensajeError = ex.Message;
                CargarFiltros();
                return View("ReporteVentas", modelo);
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.MensajeInfo = ex.Message;
                CargarFiltros();
                return View("ReporteVentas", modelo);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                CargarFiltros();
                return View("ReporteVentas", modelo);
            }
        }

        // Wrapper para exportar Excel (redirige al endpoint API que genera CSV)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportarExcel(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado)
        {
            try
            {
                using (var ctx = new GestionPubRock.AccesoADatos.Contexto())
                {
                    var p1 = new SqlParameter("@FechaInicio", (object)fechaInicio ?? DBNull.Value);
                    var p2 = new SqlParameter("@FechaFin", (object)fechaFin ?? DBNull.Value);
                    var p3 = new SqlParameter("@IdCategoria", (object)idCategoria ?? DBNull.Value);
                    var p4 = new SqlParameter("@IdTipoPago", (object)idTipoPago ?? DBNull.Value);
                    var p5 = new SqlParameter("@IdCliente", (object)idCliente ?? DBNull.Value);
                    var p6 = new SqlParameter("@IdEstado", (object)idEstado ?? DBNull.Value);

                    var rows = ctx.Database.SqlQuery<ReporteExportRowDto>(
                        "EXEC SP_PUBROCK_REPORTE_VENTAS @FechaInicio, @FechaFin, @IdCategoria, @IdTipoPago, @IdCliente, @IdEstado",
                        p1, p2, p3, p4, p5, p6).ToList();

                    // fallback: if empty, fetch without filters
                    if (rows == null || rows.Count == 0)
                    {
                        rows = ctx.Database.SqlQuery<ReporteExportRowDto>(
                            "EXEC SP_PUBROCK_REPORTE_VENTAS @FechaInicio = NULL, @FechaFin = NULL, @IdCategoria = NULL, @IdTipoPago = NULL, @IdCliente = NULL, @IdEstado = NULL"
                        ).ToList();
                    }

                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine("NumeroFactura,Fecha,Cliente,Usuario,MetodoPago,Subtotal,IVA,Descuento,Total,Estado");
                    foreach (var r in rows)
                    {
                        var fecha = (r.Fecha == DateTime.MinValue) ? "" : r.Fecha.ToString("yyyy-MM-dd HH:mm:ss");
                        sb.AppendLine($"\"{r.NumeroFactura}\",{fecha},\"{r.Cliente}\",\"{r.Usuario}\",\"{r.MetodoPago}\",{r.Subtotal},{r.IVA},{r.Descuento},{r.Total},\"{r.Estado}\"");
                    }

                    var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                    var fileName = $"ReporteVentas_{DateTime.Now:yyyyMMddHHmmss}.csv";
                    return File(bytes, "text/csv", fileName);
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return RedirectToAction("ReporteVentas");
            }
        }

        // Wrapper para exportar PDF (redirige a API; actualmente muestra mensaje si no está implementado)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportarPdf(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado)
        {
            try
            {
                using (var ctx = new GestionPubRock.AccesoADatos.Contexto())
                {
                    var p1 = new SqlParameter("@FechaInicio", (object)fechaInicio ?? DBNull.Value);
                    var p2 = new SqlParameter("@FechaFin", (object)fechaFin ?? DBNull.Value);
                    var p3 = new SqlParameter("@IdCategoria", (object)idCategoria ?? DBNull.Value);
                    var p4 = new SqlParameter("@IdTipoPago", (object)idTipoPago ?? DBNull.Value);
                    var p5 = new SqlParameter("@IdCliente", (object)idCliente ?? DBNull.Value);
                    var p6 = new SqlParameter("@IdEstado", (object)idEstado ?? DBNull.Value);

                    var rows = ctx.Database.SqlQuery<ReporteExportRowDto>(
                        "EXEC SP_PUBROCK_REPORTE_VENTAS @FechaInicio, @FechaFin, @IdCategoria, @IdTipoPago, @IdCliente, @IdEstado",
                        p1, p2, p3, p4, p5, p6).ToList();

                    if (rows == null || rows.Count == 0)
                    {
                        rows = ctx.Database.SqlQuery<ReporteExportRowDto>(
                            "EXEC SP_PUBROCK_REPORTE_VENTAS @FechaInicio = NULL, @FechaFin = NULL, @IdCategoria = NULL, @IdTipoPago = NULL, @IdCliente = NULL, @IdEstado = NULL"
                        ).ToList();
                    }

#if EXPORT_REPORTS
                    using (var ms = new MemoryStream())
                    {
                        var writer = new PdfWriter(ms);
                        var pdf = new PdfDocument(writer);
                        var doc = new Document(pdf);

                        doc.Add(new Paragraph("Nombre del negocio").SetBold());
                        doc.Add(new Paragraph($"Reporte de ventas - {DateTime.Now:yyyy-MM-dd HH:mm:ss}"));
                        doc.Add(new Paragraph(" "));

                        var table = new Table(UnitValue.CreatePercentArray(new float[] { 2, 2, 3, 3, 2, 2, 2, 2, 2, 2 })).UseAllAvailableWidth();
                        var headers = new[] { "Número factura", "Fecha", "Cliente", "Usuario", "Método de pago", "Subtotal", "IVA", "Descuento", "Total", "Estado" };
                        foreach (var h in headers) table.AddHeaderCell(new Cell().Add(new Paragraph(h)));

                        foreach (var row in rows)
                        {
                            table.AddCell(row.NumeroFactura ?? "");
                            table.AddCell(row.Fecha.ToString("yyyy-MM-dd"));
                            table.AddCell(row.Cliente ?? "");
                            table.AddCell(row.Usuario ?? "");
                            table.AddCell(row.MetodoPago ?? "");
                            table.AddCell(row.Subtotal.ToString("F2"));
                            table.AddCell(row.IVA.ToString("F2"));
                            table.AddCell(row.Descuento.ToString("F2"));
                            table.AddCell(row.Total.ToString("F2"));
                            table.AddCell(row.Estado ?? "");
                        }

                        doc.Add(table);
                        doc.Close();

                        var content = ms.ToArray();
                        var fileName = $"ReporteVentas_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                        return File(content, "application/pdf", fileName);
                    }
#else
                    // Fallback to CSV when PDF export not compiled in - ensures method always returns a file
                    var sb = new System.Text.StringBuilder();
                    sb.AppendLine("NumeroFactura,Fecha,Cliente,Usuario,MetodoPago,Subtotal,IVA,Descuento,Total,Estado");
                    foreach (var r in rows)
                    {
                        var fecha = (r.Fecha == DateTime.MinValue) ? "" : r.Fecha.ToString("yyyy-MM-dd HH:mm:ss");
                        sb.AppendLine($"\"{r.NumeroFactura}\",{fecha},\"{r.Cliente}\",\"{r.Usuario}\",\"{r.MetodoPago}\",{r.Subtotal},{r.IVA},{r.Descuento},{r.Total},\"{r.Estado}\"");
                    }
                    var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                    var fileNameCsv = $"ReporteVentas_{DateTime.Now:yyyyMMddHHmmss}.csv";
                    return File(bytes, "text/csv", fileNameCsv);
#endif
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return RedirectToAction("ReporteVentas");
            }
        }
    }
}