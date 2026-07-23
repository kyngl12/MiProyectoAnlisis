using System;
using System.Web.Mvc;
using GestionPubRock.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;
using MiPrimeraSolucionJMKK.UI.Helpers;

namespace GestionPubRock.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EgresosController : Controller
    {
        private readonly IRegistrarEgresoLN _registrarEgresoLN;
        private readonly IObtenerEgresosLN _obtenerEgresosLN;

        public EgresosController()
        {
            _registrarEgresoLN = new RegistrarEgresoLN();
            _obtenerEgresosLN = new ObtenerEgresosLN();
        }

        private string ObtenerCedulaActual()
        {
            return Session["Cedula"] as string;
        }

        // GET: Egresos
        public ActionResult Index()
        {
            var lista = _obtenerEgresosLN.Obtener(null, null);
            return View(lista);
        }

        // GET: Egresos/Agregar
        public ActionResult Agregar()
        {
            return View(new EgresoDto { Fecha = DateTime.Now.Date });
        }

        // POST: Egresos/Agregar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(EgresoDto egreso)
        {
            // Escenario 2: campos obligatorios en blanco (resaltados por Data Annotations).
            if (!ModelState.IsValid)
                return View(egreso);

            try
            {
                egreso.RegistradoPor = ObtenerCedulaActual();

                bool ok = _registrarEgresoLN.Registrar(egreso);

                if (ok)
                {
                    TempData["MensajeExito"] = "El gasto se registró de manera exitosa.";
                    return RedirectToAction("Index");
                }

                TempData["MensajeError"] = "Error en el sistema. Favor intente de nuevo.";
                return View(egreso);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                TempData["MensajeError"] = ex.Message;
                return View(egreso);
            }
        }
    }
}
