using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.EditarConfiguracion;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorId;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.RegistrarConfiguracion;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ConfiguracionComercio;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.ConfiguracionComercio.EditarConfiguracion;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorComercio;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorId;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.ConfiguracionComercio.RegistrarConfiguracion;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ConfiguracionComercioController : Controller
    {
        private readonly IObtenerConfiguracionPorComercioLN _obtenerConfiguracionPorComercioLN;
        private readonly IObtenerConfiguracionPorIdLN _obtenerConfiguracionPorIdLN;
        private readonly IRegistrarConfiguracionLN _registrarConfiguracionLN;
        private readonly IEditarConfiguracionLN _editarConfiguracionLN;

        public ConfiguracionComercioController()
        {
            _obtenerConfiguracionPorComercioLN = new ObtenerConfiguracionPorComercioLN();
            _obtenerConfiguracionPorIdLN = new ObtenerConfiguracionPorIdLN();
            _registrarConfiguracionLN = new RegistrarConfiguracionLN();
            _editarConfiguracionLN = new EditarConfiguracionLN();
        }

        public ActionResult ObtenerConfiguracionPorComercio(int idComercio)
        {
            List<ConfiguracionComercioDto> configuraciones = _obtenerConfiguracionPorComercioLN.Obtener(idComercio);
            ViewBag.IdComercio = idComercio;
            return View(configuraciones);
        }

        public ActionResult AgregarConfiguracion(int idComercio)
        {
            var modelo = new ConfiguracionComercioDto
            {
                IdComercio = idComercio
            };
            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarConfiguracion(ConfiguracionComercioDto modelo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(modelo);
                }

                bool resultado = _registrarConfiguracionLN.Registrar(modelo);

                if (!resultado)
                {
                    TempData["Error"] = "Ya existe una configuración para este comercio";
                    return View(modelo);
                }

                TempData["Exito"] = "Configuración registrada exitosamente";
                return RedirectToAction("ObtenerConfiguracionPorComercio", new { idComercio = modelo.IdComercio });
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Error al registrar la configuración: " + ex.Message;
                return View(modelo);
            }
        }

        public ActionResult EditarConfiguracion(int id)
        {
            ConfiguracionComercioDto configuracion = _obtenerConfiguracionPorIdLN.Obtener(id);
            if (configuracion == null)
            {
                return HttpNotFound();
            }
            return View(configuracion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarConfiguracion(ConfiguracionComercioDto modelo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(modelo);
                }

                bool resultado = _editarConfiguracionLN.Editar(modelo);

                if (!resultado)
                {
                    TempData["Error"] = "No se pudo editar la configuración";
                    return View(modelo);
                }

                TempData["Exito"] = "Configuración editada exitosamente";
                return RedirectToAction("ObtenerConfiguracionPorComercio", new { idComercio = modelo.IdComercio });
            }
            catch (System.Exception ex)
            {
                TempData["Error"] = "Error al editar la configuración: " + ex.Message;
                return View(modelo);
            }
        }
    }
}
