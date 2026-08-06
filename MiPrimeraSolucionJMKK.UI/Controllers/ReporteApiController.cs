using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.IO;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace GestionPubRock.UI.Controllers
{
    // API controller para devolver JSON simple para el frontend del dashboard
    public class ReporteApiController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
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
                    var prodMasVendido = (ventasPorProducto != null && ventasPorProducto.Count > 0) ? ventasPorProducto[0].Nombre : string.Empty;

                    var desde = today.AddDays(-29);
                    var endDayInclusive = endToday;
                    var ventasDias = ctx.Facturas.Where(f => f.FechaFactura >= desde && f.FechaFactura < endDayInclusive && f.IdEstado == 1)
                        .GroupBy(f => System.Data.Entity.DbFunctions.TruncateTime(f.FechaFactura))
                        .Select(g => new { Fecha = g.Key.Value, Total = g.Sum(x => x.MontoTotal) })
                        .ToList();

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

                    var ventasPorDiaResult = ventasDias.Select(v => new { fecha = v.Fecha.ToString("yyyy-MM-dd"), total = v.Total }).ToList();
                    var ventasPorProductoResult = ventasPorProducto.Select(v => new { nombre = v.Nombre, cantidad = v.Cant }).Take(10).ToList();

                    // Método de pago más usado
                    var metodoTop = ctx.Facturas.Where(f => f.IdEstado == 1)
                        .GroupBy(f => f.IdTipoPago)
                        .Select(g => new { IdTipoPago = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                        .FirstOrDefault();

                    string metodoMasUsado = string.Empty;
                    if (metodoTop != null && metodoTop.IdTipoPago != null)
                    {
                        var tp = ctx.TiposPago.Find(metodoTop.IdTipoPago);
                        metodoMasUsado = tp != null ? tp.Descripcion : string.Empty;
                    }

                    return Json(new
                    {
                        totalToday,
                        totalMonth,
                        prodMasVendido,
                        metodoMasUsado,
                        ventasPorDia = ventasPorDiaResult,
                        ventasPorProducto = ventasPorProductoResult
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExportarExcelCsv(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado)
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

                    var rows = ctx.Database.SqlQuery<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes.ReporteExportRowDto>(
                        "EXEC SP_PUBROCK_REPORTE_VENTAS @FechaInicio, @FechaFin, @IdCategoria, @IdTipoPago, @IdCliente, @IdEstado",
                        p1, p2, p3, p4, p5, p6).ToList();

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
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

#if EXPORT_REPORTS
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExportarExcelXlsx(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado)
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

                    var rows = ctx.Database.SqlQuery<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes.ReporteExportRowDto>(
                        "EXEC SP_PUBROCK_REPORTE_VENTAS @FechaInicio, @FechaFin, @IdCategoria, @IdTipoPago, @IdCliente, @IdEstado",
                        p1, p2, p3, p4, p5, p6).ToList();

                    using (var ms = new MemoryStream())
                    using (var wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("ReporteVentas");
                        ws.Cell(1, 1).Value = "Nombre del negocio";
                        ws.Cell(2, 1).Value = "Reporte de ventas";
                        ws.Cell(3, 1).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        var headerRow = 5;
                        var headers = new[] { "Número factura", "Fecha", "Cliente", "Usuario", "Método de pago", "Subtotal", "IVA", "Descuento", "Total", "Estado" };
                        for (int i = 0; i < headers.Length; i++) ws.Cell(headerRow, i + 1).Value = headers[i];

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
                        wb.SaveAs(ms);
                        var content = ms.ToArray();
                        var fileName = $"ReporteVentas_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExportarPdfGenerate(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado)
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

                    var rows = ctx.Database.SqlQuery<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes.ReporteExportRowDto>(
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
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
#endif

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExportarPdfInfo(DateTime? fechaInicio, DateTime? fechaFin, int? idCategoria, int? idTipoPago, int? idCliente, int? idEstado)
        {
            // Inform the user how to enable full PDF export
            return Content("Exportar PDF requiere que se restauren paquetes NuGet (iText7 o QuestPDF) y que la opción EXPORT_REPORTS esté activada en las propiedades del proyecto.", "text/plain");
        }


        [HttpGet]
        [AllowAnonymous]
        public ActionResult FacturasRecent(int top = 50)
        {
            try
            {
                using (var ctx = new GestionPubRock.AccesoADatos.Contexto())
                {
                    var sql = "SELECT TOP (@p0) ID_FACTURA AS IdFactura, FECHA_FACTURA AS FechaFactura, MONTO_TOTAL AS MontoTotal, ID_TIPO_PAGO AS IdTipoPago, ID_ESTADO AS IdEstado, ID_VENTA AS IdVenta FROM PUBROCK_FACTURA_TB ORDER BY FECHA_FACTURA DESC";
                    var param = new SqlParameter("@p0", top);
                    var rows = ctx.Database.SqlQuery<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes.FacturaRowDto>(sql, param).ToList();
                    return Json(new { count = rows.Count, rows = rows }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
