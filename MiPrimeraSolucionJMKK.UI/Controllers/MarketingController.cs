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
        private IObtenerTodasLasPublicacionesLN _obtenerTodasLasPublicacionesLN;
        private IRegistrarPublicacionLN _registrarPublicacionLN;
        private IEditarPublicacionLN _editarPublicacionLN;

        public MarketingController()
        {
            _obtenerTodasLasPublicacionesLN = new ObtenerTodasLasPublicacionesLN();
            _registrarPublicacionLN = new RegistrarPublicacionLN();
            _editarPublicacionLN = new EditarPublicacionLN();
        }

        public ActionResult Index() => RedirectToAction("ObtenerTodasLasPublicaciones");

            public ActionResult ObtenerTodasLasPublicaciones()
            {
                var lista = _obtenerTodasLasPublicacionesLN.Obtener();
                return View(lista);
            }

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
                    bool seRegistro = _registrarPublicacionLN.Registrar(publicacion);

                    if (seRegistro)
                    {
                        TempData["MensajeExito"] = "La publicación se registró de manera exitosa.";
                        return RedirectToAction("ObtenerTodasLasPublicaciones");
                    }

                    TempData["MensajeError"] = "Error al registrar la publicación.";
                    return View(publicacion);
                }
                catch (ArgumentException aex)
                {
                    TempData["MensajeError"] = aex.Message;
                    return View(publicacion);
                }
                catch (Exception ex)
                {
                    MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.Log(ex);
                    TempData["MensajeError"] = "Error en el sistema. Revise logs.";
                    return View(publicacion);
                }
            }

            public ActionResult EditarPublicacion(int id)
            {
                var lista = _obtenerTodasLasPublicacionesLN.Obtener();
                var publicacion = lista.FirstOrDefault(p => p.IdPublicacion == id);

                return View(publicacion);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult EditarPublicacion(MarketingDto publicacion)
            {
                if (!ModelState.IsValid)
                    return View(publicacion);

                try
                {
                    bool seEdito = _editarPublicacionLN.Editar(publicacion);

                    if (seEdito)
                    {
                        if (publicacion.IdEstado == 2)
                            TempData["MensajeExito"] = "La publicación fue desactivada exitosamente.";
                        else
                            TempData["MensajeExito"] = "La publicación fue editada de manera exitosa.";

                        return RedirectToAction("ObtenerTodasLasPublicaciones");
                    }

                    TempData["MensajeError"] = "Error al editar la publicación.";
                    return View(publicacion);
                }
                catch (ArgumentException aex)
                {
                    TempData["MensajeError"] = aex.Message;
                    return View(publicacion);
                }
                catch (Exception ex)
                {
                    MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.Log(ex);
                    TempData["MensajeError"] = "Error en el sistema. Revise logs.";
                    return View(publicacion);
                }
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult DesactivarPublicacion(int id)
            {
                try
                {
                    var lista = _obtenerTodasLasPublicacionesLN.Obtener();
                    var publicacion = lista.FirstOrDefault(p => p.IdPublicacion == id);

                    if (publicacion == null)
                    {
                        TempData["MensajeError"] = "La publicación no existe.";
                        return RedirectToAction("ObtenerTodasLasPublicaciones");
                    }

                    publicacion.IdEstado = 2; 

                    _editarPublicacionLN.Editar(publicacion);

                    TempData["MensajeExito"] = "La publicación fue desactivada exitosamente.";
                    return RedirectToAction("ObtenerTodasLasPublicaciones");
                }
                catch (Exception ex)
                {
                    MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.Log(ex);
                    TempData["MensajeError"] = "Error en el sistema. Revise logs.";
                    return RedirectToAction("ObtenerTodasLasPublicaciones");
                }
            }
        }
    }
    
