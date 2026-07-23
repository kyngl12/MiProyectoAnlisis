using System;
using System.Web.Mvc;
using GestionPubRock.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;
using MiPrimeraSolucionJMKK.UI.Helpers;

namespace GestionPubRock.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class BalanceController : Controller
    {
        private readonly IGenerarBalanceLN _generarBalanceLN;

        public BalanceController()
        {
            _generarBalanceLN = new GenerarBalanceLN();
        }

        // GET: Balance
        public ActionResult Index()
        {
            return View(new BalanceDto());
        }

        // POST: Balance/Generar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generar(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var balance = _generarBalanceLN.Generar(fechaInicio, fechaFin);
                TempData["BalanceFechaInicio"] = fechaInicio;
                TempData["BalanceFechaFin"] = fechaFin;
                return View("Index", balance);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                TempData["MensajeError"] = ex.Message;
                return View("Index", new BalanceDto { FechaInicio = fechaInicio, FechaFin = fechaFin });
            }
        }

        // GET: Balance/DescargarPdf
        public ActionResult DescargarPdf(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var balance = _generarBalanceLN.Generar(fechaInicio, fechaFin);

                var pdf = new SimplePdfBuilder("Balance de Pérdidas y Ganancias - PubRock");
                pdf.AgregarLinea("Período: " + fechaInicio.ToString("dd/MM/yyyy") + " al " + fechaFin.ToString("dd/MM/yyyy"));
                pdf.AgregarSeparador();
                pdf.AgregarLinea("Total de ingresos: " + balance.TotalIngresos.ToString("C2"));
                pdf.AgregarLinea("Total de egresos: " + balance.TotalEgresos.ToString("C2"));
                pdf.AgregarLinea((balance.EsPerdida ? "Pérdida del período: " : "Utilidad neta: ") + balance.UtilidadNeta.ToString("C2"));
                pdf.AgregarSeparador();
                pdf.AgregarLinea("Desglose por categoría:");

                foreach (var fila in balance.Desglose)
                    pdf.AgregarLinea("  [" + fila.Tipo + "] " + fila.Categoria + ": " + fila.Monto.ToString("C2"));

                byte[] archivo = pdf.Generar();

                string nombreArchivo = "Balance_" + fechaInicio.ToString("yyyyMMdd") + "_" + fechaFin.ToString("yyyyMMdd") + ".pdf";
                return File(archivo, "application/pdf", nombreArchivo);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                TempData["MensajeError"] = ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
