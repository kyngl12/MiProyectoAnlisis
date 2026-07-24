using System;
using System.Web.Mvc;
using GestionPubRock.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;
using MiPrimeraSolucionJMKK.UI.Helpers;

namespace GestionPubRock.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CuentasPorPagarController : Controller
    {
        private readonly IRegistrarCuentaPorPagarLN _registrarCuentaPorPagarLN;
        private readonly IObtenerCuentasPorPagarLN _obtenerCuentasPorPagarLN;
        private readonly IMarcarCuentaPorPagarComoPagadaLN _marcarComoPagadaLN;

        public CuentasPorPagarController()
        {
            _registrarCuentaPorPagarLN = new RegistrarCuentaPorPagarLN();
            _obtenerCuentasPorPagarLN = new ObtenerCuentasPorPagarLN();
            _marcarComoPagadaLN = new MarcarCuentaPorPagarComoPagadaLN();
        }

        private string ObtenerCedulaActual()
        {
            // Intenta obtener la cédula de la sesión
            string cedula = Session["Cedula"] as string;

            // Si la sesión no tiene la cédula, intenta obtenerla de los claims
            if (string.IsNullOrWhiteSpace(cedula) && User?.Identity?.IsAuthenticated == true)
            {
                cedula = User.Identity.Name;
            }

            return cedula;
        }

        // GET: CuentasPorPagar
        public ActionResult Index()
        {
            var lista = _obtenerCuentasPorPagarLN.Obtener();

            // Escenario 3: aviso de cuentas proximas a vencer (2 dias antes).
            ViewBag.CuentasPorVencer = lista.FindAll(c => c.EstadoPago == "Pendiente" && c.DiasParaVencer == 2);

            return View(lista);
        }

        // GET: CuentasPorPagar/Agregar
        public ActionResult Agregar()
        {
            return View(new CuentaPorPagarDto { FechaVencimiento = DateTime.Now.Date.AddDays(15) });
        }

        // POST: CuentasPorPagar/Agregar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(CuentaPorPagarDto cuenta)
        {
            if (!ModelState.IsValid)
                return View(cuenta);

            try
            {
                cuenta.RegistradoPor = ObtenerCedulaActual();

                bool ok = _registrarCuentaPorPagarLN.Registrar(cuenta);

                if (ok)
                {
                    TempData["MensajeExito"] = "La cuenta por pagar se registró correctamente.";
                    return RedirectToAction("Index");
                }

                TempData["MensajeError"] = "Error en el sistema. Favor intente de nuevo.";
                return View(cuenta);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                TempData["MensajeError"] = ex.Message;
                return View(cuenta);
            }
        }

        // POST: CuentasPorPagar/MarcarComoPagado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarcarComoPagado(int id)
        {
            try
            {
                _marcarComoPagadaLN.MarcarComoPagada(id, ObtenerCedulaActual());
                TempData["MensajeExito"] = "La deuda se liquidó de manera exitosa.";
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                TempData["MensajeError"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
