using GestionPubRock.LogicaDeNegocio.Marketing.EditarPublicacion;
using GestionPubRock.LogicaDeNegocio.Marketing.ObtenerTodasLasPublicaciones;
using GestionPubRock.LogicaDeNegocio.Marketing.RegistrarPublicacion;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Marketing.EditarPublicacion;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Marketing.ObtenerTodasLasPublicaciones;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Marketing.RegistrarPublicacion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Marketing;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class MarketingController : Controller
    {
        private readonly IObtenerTodasLasPublicacionesLN _obtenerTodasLasPublicacionesLN;
        private readonly IRegistrarPublicacionLN _registrarPublicacionLN;
        private readonly IEditarPublicacionLN _editarPublicacionLN;

        public MarketingController()
        {
            _obtenerTodasLasPublicacionesLN = new ObtenerTodasLasPublicacionesLN();
            _registrarPublicacionLN = new RegistrarPublicacionLN();
            _editarPublicacionLN = new EditarPublicacionLN();
        }

        public ActionResult Index()
        {
            return RedirectToAction("ObtenerTodasLasPublicaciones");
        }

        // LISTADO
        public ActionResult ObtenerTodasLasPublicaciones()
        {
            var lista = _obtenerTodasLasPublicacionesLN.Obtener();
            return View(lista); // ✔ LISTA
        }

        // CREATE
        public ActionResult AgregarPublicacion()
        {
            return View(new MarketingDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarPublicacion(MarketingDto publicacion)
        {
            if (!ModelState.IsValid)
                return View(publicacion);

            try
            {
                bool ok = _registrarPublicacionLN.Registrar(publicacion);

                if (ok)
                {
                    TempData["MensajeExito"] = "Publicación registrada correctamente.";
                    return RedirectToAction("ObtenerTodasLasPublicaciones");
                }

                TempData["MensajeError"] = "Error al registrar.";
                return View(publicacion);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return View(publicacion);
            }
        }

        // EDIT GET
        public ActionResult EditarPublicacion(int id)
        {
            var lista = _obtenerTodasLasPublicacionesLN.Obtener();
            var publicacion = lista.FirstOrDefault(x => x.IdPublicacion == id);

            if (publicacion == null)
            {
                TempData["MensajeError"] = "Publicación no encontrada.";
                return RedirectToAction("ObtenerTodasLasPublicaciones");
            }

            return View(publicacion); // ✔ SOLO 1 DTO
        }

        // EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarPublicacion(MarketingDto publicacion)
        {
            if (!ModelState.IsValid)
                return View(publicacion);

            try
            {
                bool ok = _editarPublicacionLN.Editar(publicacion);

                if (ok)
                {
                    TempData["MensajeExito"] =
                        publicacion.IdEstado == 2
                        ? "Publicación desactivada."
                        : "Publicación actualizada.";

                    return RedirectToAction("ObtenerTodasLasPublicaciones");
                }

                TempData["MensajeError"] = "Error al actualizar.";
                return View(publicacion);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return View(publicacion);
            }
        }

        // DELETE LOGIC (soft delete)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DesactivarPublicacion(int id)
        {
            try
            {
                var lista = _obtenerTodasLasPublicacionesLN.Obtener();
                var pub = lista.FirstOrDefault(x => x.IdPublicacion == id);

                if (pub == null)
                {
                    TempData["MensajeError"] = "No existe la publicación.";
                    return RedirectToAction("ObtenerTodasLasPublicaciones");
                }

                pub.IdEstado = 2;

                _editarPublicacionLN.Editar(pub);
                TempData["MensajeExito"] = "Publicación desactivada.";
                return RedirectToAction("ObtenerTodasLasPublicaciones");
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = "Error interno.";
                return RedirectToAction("ObtenerTodasLasPublicaciones");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActivarPublicacion(int id)
        {
            try
            {
                var lista = _obtenerTodasLasPublicacionesLN.Obtener();
                var pub = lista.FirstOrDefault(x => x.IdPublicacion == id);

                if (pub == null)
                {
                    TempData["MensajeError"] = "No existe la publicación.";
                    return RedirectToAction("ObtenerTodasLasPublicaciones");
                }

                pub.IdEstado = 1; // Activo

                _editarPublicacionLN.Editar(pub);

                TempData["MensajeExito"] = "Publicación activada.";
                return RedirectToAction("ObtenerTodasLasPublicaciones");
            }
            catch
            {
                TempData["MensajeError"] = "Error interno.";
                return RedirectToAction("ObtenerTodasLasPublicaciones");
            }
        }
    }
}