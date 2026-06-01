using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Comercios.ObtenerComercioPorId;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Comercios.ObtenerTodosLosComercios;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Comercios.RegistrarComercio;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.ReportesMensuales;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Comercios.EditarComercio;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Comercios.ObtenerComercioPorId;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Comercios.ObtenerTodosLosComercios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Comercios.RegistrarComercio;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = ("Administrador"))]
    public class ComerciosController : Controller
    {
        private IObtenerTodosLosComerciosLN _obtenerTodosLosComerciosLN;
        private IObtenerComercioPorIdLN _obtenerComercioPorIdLN;
        private IRegistrarComercioLN _registrarComercioLN;
        private IEditarComercioLN _editarComercioLN;

        public ComerciosController()
        {
            _obtenerTodosLosComerciosLN = new ObtenerTodosLosComerciosLN();
            _obtenerComercioPorIdLN = new ObtenerComercioPorIdLN();
            _registrarComercioLN = new RegistrarComercioLN();
            _editarComercioLN = new EditarComercioLN();
        }

        public ActionResult ObtenerTodosLosComercios()
        {
            List<ComercioDto> laListaDeComercios = _obtenerTodosLosComerciosLN.Obtener();
            return View(laListaDeComercios);
        }

        public ActionResult DetallesDelComercio(int id)
        {
            ComercioDto elComercio = _obtenerComercioPorIdLN.Obtener(id);
            return View(elComercio);
        }

        // GET: Agregar Comercio
        public ActionResult AgregarComercio()
        {
            return View(new ComercioDto());
        }

        [HttpPost]
        public ActionResult AgregarComercio(ComercioDto elComercioAGuardar)
        {
            try
            {
                bool seAgrego = _registrarComercioLN.Registrar(elComercioAGuardar);

                if (seAgrego)
                    return RedirectToAction("ObtenerTodosLosComercios");

                TempData["Mensaje"] = "Error al agregar. Ya existe un comercio con esa identificación";
                return RedirectToAction("AgregarComercio");
            }
            catch
            {
                TempData["Mensaje"] = "Ocurrió un error inesperado";
                return RedirectToAction("AgregarComercio");
            }
        }

        public ActionResult EditarComercio(int id)
        {
            ComercioDto elComercio = _obtenerComercioPorIdLN.Obtener(id);
            return View(elComercio);
        }

        [HttpPost]
        public ActionResult EditarComercio(int id, ComercioDto elComercioParaActualizar)
        {
            try
            {
                bool seActualizo = _editarComercioLN.Editar(elComercioParaActualizar);

                if (seActualizo)
                    return RedirectToAction("ObtenerTodosLosComercios");

                return View(elComercioParaActualizar);
            }
            catch
            {
                return View(elComercioParaActualizar);
            }
        }
    }
}