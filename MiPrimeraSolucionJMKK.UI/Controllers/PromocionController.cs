using GestionPubRock.LogicaDeNegocio.Promocion;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Promocion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Promocion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GestionPubRock.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PromocionController : Controller
    {
        private readonly IRegistrarPromocionLN _registrarPromocionLN;
        private readonly IEditarPromocionLN _editarPromocionLN;
        private readonly IDesactivarPromocionLN _desactivarPromocionLN;
        private readonly IObtenerTodasLasPromocionesLN _obtenerTodasLasPromocionesLN;


        public PromocionController()
        {
            _registrarPromocionLN = new RegistrarPromocionLN();
            _editarPromocionLN = new EditarPromocionLN();
            _desactivarPromocionLN = new DesactivarPromocionLN();
            _obtenerTodasLasPromocionesLN = new ObtenerTodasLasPromocionesLN();
        }

        public ActionResult Index()
        {
            return RedirectToAction("ObtenerTodasLasPromociones");
        }

        // LISTADO
        public ActionResult ObtenerTodasLasPromociones()
        {
            var lista = _obtenerTodasLasPromocionesLN.Obtener();
            return View(lista);
        }

        // CREATE GET
        public ActionResult AgregarPromocion()
        {
            return View(new PromocionDto());
        }

        // CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarPromocion(PromocionDto promocion)
        {
            if (!ModelState.IsValid)
                return View(promocion);

            try
            {
                bool ok = _registrarPromocionLN.Registrar(promocion);

                if (ok)
                {
                    TempData["MensajeExito"] = "Promoción registrada correctamente.";
                    return RedirectToAction("ObtenerTodasLasPromociones");
                }

                TempData["MensajeError"] = "Error al registrar la promoción.";
                return View(promocion);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return View(promocion);
            }
        }

        // EDIT GET
        public ActionResult EditarPromocion(int id)
        {
            var lista = _obtenerTodasLasPromocionesLN.Obtener();
            var promocion = lista.FirstOrDefault(x => x.IdPromocion == id);

            if (promocion == null)
            {
                TempData["MensajeError"] = "La promoción no se encuentra registrada.";
                return RedirectToAction("ObtenerTodasLasPromociones");
            }

            return View(promocion);
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPromocion(PromocionDto promocion)
        {
            if (!ModelState.IsValid)
                return View(promocion);

            try
            {
                bool ok = _editarPromocionLN.Editar(promocion);

                if (ok)
                {
                    TempData["MensajeExito"] = "Promoción actualizada correctamente.";
                    return RedirectToAction("ObtenerTodasLasPromociones");
                }

                TempData["MensajeError"] = "Error al actualizar la promoción.";
                return View(promocion);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return View(promocion);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Buscar(string criterio, int? idEstado)
        {
            try
            {
                var lista = _obtenerTodasLasPromocionesLN.Obtener(criterio, idEstado);

                ViewBag.Criterio = criterio;
                ViewBag.IdEstado = idEstado;

                if (lista == null || !lista.Any())
                {
                    if (idEstado.HasValue)
                    {
                        TempData["MensajeInfo"] = "No existen promociones con el estado seleccionado";
                    }
                    else
                    {
                        TempData["MensajeInfo"] = "No se encontraron promociones con el criterio ingresado.";
                    }
                }

                return View("ObtenerTodasLasPromociones", lista);
            }
            catch
            {
                TempData["MensajeError"] = "Error en el sistema. Por favor intente nuevamente.";
                return RedirectToAction("ObtenerTodasLasPromociones");
            }
        }


        // DESACTIVAR
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DesactivarPromocion(int id)
        {
            try
            {
                _desactivarPromocionLN.Desactivar(id);

                TempData["MensajeExito"] = "Promoción desactivada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
            }

            return RedirectToAction("ObtenerTodasLasPromociones");
        }
    }
}
