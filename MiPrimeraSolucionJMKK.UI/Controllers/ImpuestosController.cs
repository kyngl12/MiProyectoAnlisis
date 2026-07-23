using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GestionPubRock.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;
using MiPrimeraSolucionJMKK.UI.Helpers;

namespace GestionPubRock.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ImpuestosController : Controller
    {
        private readonly IObtenerImpuestosLN _obtenerImpuestosLN;
        private readonly IActualizarImpuestoLN _actualizarImpuestoLN;
        private readonly ICalcularImpuestosLN _calcularImpuestosLN;

        public ImpuestosController()
        {
            _obtenerImpuestosLN = new ObtenerImpuestosLN();
            _actualizarImpuestoLN = new ActualizarImpuestoLN();
            _calcularImpuestosLN = new CalcularImpuestosLN();
        }

        private string ObtenerCedulaActual()
        {
            return Session["Cedula"] as string;
        }

        // GET: Impuestos
        public ActionResult Index()
        {
            List<ImpuestoDto> lista = _obtenerImpuestosLN.Obtener();
            return View(lista);
        }

        // POST: Impuestos/Actualizar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Actualizar(ImpuestoDto impuesto)
        {
            try
            {
                impuesto.ActualizadoPor = ObtenerCedulaActual();

                _actualizarImpuestoLN.Actualizar(impuesto);

                TempData["MensajeExito"] = "La tasa de impuesto se actualizó correctamente. Los cambios aplican de inmediato a las ventas futuras.";
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                TempData["MensajeError"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        // POST: Impuestos/Calcular (calculadora de desglose IVA/Servicio de una orden)
        [HttpPost]
        public JsonResult Calcular(decimal subtotal, bool exento)
        {
            try
            {
                var resultado = _calcularImpuestosLN.Calcular(subtotal, exento);
                return Json(new
                {
                    success = true,
                    subtotal = resultado.Subtotal,
                    porcentajeIva = resultado.PorcentajeIva,
                    porcentajeServicio = resultado.PorcentajeServicio,
                    montoIva = resultado.MontoIva,
                    montoServicio = resultado.MontoServicio,
                    total = resultado.Total,
                    exento = resultado.Exento
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
