using GestionPubRock.LogicaDeNegocio.Reportes;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Reportes;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionPubRock.UI.Controllers
{
    
    [Authorize(Roles = "Administrador")]
    public class ReporteController : Controller
    {
        private readonly IReporteVentasLN _reporteVentasLN;

        public ReporteController()
        {
            _reporteVentasLN = new ReporteVentasLN();
        }

        // GET: Reporte/ReporteVentas
        public ActionResult ReporteVentas()
        {
            var modelo = new ReporteVentasDto
            {
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today
            };

            return View(modelo);
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
                ReporteVentasDto reporte =
                    _reporteVentasLN.Generar(fechaInicio, fechaFin);

                TempData["MensajeExito"] =
                    "Reporte generado correctamente.";

                return View(reporte);
            }
            catch (ArgumentException ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View(modelo);
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.MensajeInfo = ex.Message;
                return View(modelo);
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                return View(modelo);
            }
        }
    }
}