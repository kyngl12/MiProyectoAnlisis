using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.GenerarReporteMensual;
using MiPrimeraSolucionJMKK.Abstracciones.AccesoADatos.ReportesMensuales.ObtenerTodosLosReportesMensuales;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ReportesMensuales;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.ReportesMensuales.GenerarReporteMensual;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.ReportesMensuales.ObtenerTodosLosReportesMensuales;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = ("Administrador"))]
    public class ReportesMensualesController : Controller
    {
        IObtenerTodosLosReportesMensualesLN _obtenerTodosLosReportesMensualesLN;
        IGenerarReportesMensualesLN _generarReportesMensualesLN;

        public ReportesMensualesController()
        {
            _obtenerTodosLosReportesMensualesLN = new ObtenerTodosLosReportesMensualesLN();
            _generarReportesMensualesLN = new GenerarReportesMensualesLN();
        }

        public ActionResult ObtenerTodosLosReportesMensuales()
        {
            List<ReportesMensualesDto> laListaDeReportes = _obtenerTodosLosReportesMensualesLN.Obtener();

            return View(laListaDeReportes);
        }

        [HttpPost]
        public ActionResult GenerarReportesMensuales()
        {
            _generarReportesMensualesLN.GenerarReportesMensuales();

            return RedirectToAction("ObtenerTodosLosReportesMensuales");
        }
    }
}