using System;
using System.Web.Mvc;
using GestionPubRock.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.UI.Helpers;

namespace GestionPubRock.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CierreCajaController : Controller
    {
        private readonly IRealizarCierreCajaLN _realizarCierreCajaLN;

        public CierreCajaController()
        {
            _realizarCierreCajaLN = new RealizarCierreCajaLN();
        }

        private string ObtenerCedulaActual()
        {
            return Session["Cedula"] as string;
        }

        // GET: CierreCaja
        public ActionResult Index()
        {
            var resumen = _realizarCierreCajaLN.ObtenerResumenDelDia(DateTime.Now.Date);
            return View(resumen);
        }

        // POST: CierreCaja/Cerrar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cerrar(decimal montoContado)
        {
            try
            {
                var resultado = _realizarCierreCajaLN.Cerrar(DateTime.Now.Date, montoContado, ObtenerCedulaActual());

                TempData["MensajeExito"] = resultado.ResultadoCuadre;
                TempData["ResultadoCierre"] = resultado.ResultadoCuadre;

                return RedirectToAction("Index");
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
