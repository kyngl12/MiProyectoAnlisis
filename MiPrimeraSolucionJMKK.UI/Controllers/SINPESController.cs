using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.SINPES.RegistrarSINPES;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.SINPES.RegistrarSINPES;
using System;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = ("Administrador"))]
    public class SINPESController : Controller
    {

        private IRegistrarSINPESLN _registrarSINPESLN;

        public SINPESController()
        {
            _registrarSINPESLN = new RegistrarSINPESLN();
        }

        public ActionResult RegistrarSINPES()
        {
            return View(new SINPESDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarSINPES(SINPESDto elPagoSINPESAGuardar)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["MensajeError"] = "Por favor complete todos los campos requeridos correctamente.";
                    return View(elPagoSINPESAGuardar);
                }

                bool seRegistro = _registrarSINPESLN.Registrar(elPagoSINPESAGuardar);

                if (seRegistro)
                {
                    TempData["Exito"] = "SINPE registrado exitosamente";
                    return RedirectToAction("RegistrarSINPES");
                }
                else
                {
                    TempData["MensajeError"] = "No se pudo registrar el SINPE. Verifique que la caja de destino exista y esté activa.";
                    return View(elPagoSINPESAGuardar);
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error al registrar el SINPE: " + ex.Message;
                return View(elPagoSINPESAGuardar);
            }
        }

    }
}