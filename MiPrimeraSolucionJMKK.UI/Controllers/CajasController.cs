using GestionPubRock.AccesoADatos;
using Microsoft.AspNet.Identity;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.EditarCaja;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCajaPorId;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCajasPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.ObtenerSINPESPorTelefonoCaja;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.RegistrarCaja;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.SINPESApi;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPESApi;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Cajas.EditarCaja;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Cajas.ObtenerCajaPorId;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Cajas.ObtenerCajasPorComercio;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Cajas.ObtenerSINPESPorTelefonoCaja;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Cajas.RegistrarCaja;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.SINPESApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{

    [Authorize(Roles = "Administrador, Cajero")]
    public class CajasController : Controller
    {
        private readonly IObtenerCajasPorComercioLN _obtenerCajasPorComercioLN;
        private readonly IObtenerCajaPorIdLN _obtenerCajaPorIdLN;
        private readonly IRegistrarCajaLN _registrarCajaLN;
        private readonly IEditarCajaLN _editarCajaLN;
        private readonly IObtenerSINPESPorTelefonoCajaLN _obtenerSINPESPorTelefonoCajaLN;
        private readonly ISINPESApiLN _sinpeApiLN;

        public CajasController()
        {
            _obtenerCajasPorComercioLN = new ObtenerCajasPorComercioLN();
            _obtenerCajaPorIdLN = new ObtenerCajaPorIdLN();
            _registrarCajaLN = new RegistrarCajaLN();
            _editarCajaLN = new EditarCajaLN();
            _obtenerSINPESPorTelefonoCajaLN = new ObtenerSINPESPorTelefonoCajaLN();
            _sinpeApiLN = new SinpeApiLN();
        }

        private int? ObtenerIdComercioDelUsuario()
        {
            if (User.IsInRole("Cajero"))
            {
                string userId = User.Identity.GetUserId();
                using (var contexto = new Contexto())
                {
                }
            }
            return null;
        }

        public ActionResult ObtenerTodasLasCajas()
        {
            if (User.IsInRole("Cajero"))
            {
                int? idComercio = ObtenerIdComercioDelUsuario();
                if (idComercio.HasValue)
                {
                    return RedirectToAction("ObtenerCajasPorComercio", new { idComercio = idComercio.Value });
                }
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("ObtenerTodosLosComercios", "Comercios");
        }

        public ActionResult ObtenerCajasPorComercio(int idComercio)
        {
            if (User.IsInRole("Cajero"))
            {
                int? idComercioUsuario = ObtenerIdComercioDelUsuario();
                if (!idComercioUsuario.HasValue || idComercioUsuario.Value != idComercio)
                {
                    TempData["Error"] = "No tiene permisos para ver las cajas de este comercio";
                    return RedirectToAction("ObtenerTodasLasCajas");
                }
            }

            List<CajaDto> laListaDeCajas = _obtenerCajasPorComercioLN.Obtener(idComercio);
            ViewBag.IdComercio = idComercio;
            return View(laListaDeCajas);
        }

        // GET: Agregar Caja
        public ActionResult AgregarCaja(int? idComercio)
        {
            if (!idComercio.HasValue)
                return RedirectToAction("ObtenerTodosLosComercios", "Comercios");

            CajaDto laCaja = new CajaDto
            {
                IdComercio = idComercio.Value
            };

            return View(laCaja);
        }

        [HttpPost]
        public ActionResult AgregarCaja(CajaDto laCajaAGuardar)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(laCajaAGuardar);

                bool seAgrego = _registrarCajaLN.Registrar(laCajaAGuardar);

                if (seAgrego)
                    return RedirectToAction("ObtenerCajasPorComercio", new { idComercio = laCajaAGuardar.IdComercio });

                ViewBag.MensajeError = "No fue posible registrar la caja. Verifique si el teléfono activo ya existe o si el nombre ya está repetido en ese comercio.";
                return View(laCajaAGuardar);
            }
            catch
            {
                ViewBag.MensajeError = "Ocurrió un error inesperado al registrar la caja.";
                return View(laCajaAGuardar);
            }
        }

        public ActionResult EditarCaja(int id)
        {
            CajaDto laCaja = _obtenerCajaPorIdLN.Obtener(id);
            if (laCaja == null) return HttpNotFound();
            return View(laCaja);
        }

        [HttpPost]
        public ActionResult EditarCaja(int id, CajaDto laCajaAEditar)
        {
            try
            {
                laCajaAEditar.IdCaja = id;

                if (!ModelState.IsValid)
                    return View(laCajaAEditar);

                bool seEdito = _editarCajaLN.Editar(laCajaAEditar);

                if (seEdito)
                    return RedirectToAction("ObtenerCajasPorComercio", new { idComercio = laCajaAEditar.IdComercio });

                ViewBag.MensajeError = "No fue posible editar la caja. Revise si el teléfono activo ya existe o si el nombre ya está repetido en ese comercio.";
                return View(laCajaAEditar);
            }
            catch
            {
                ViewBag.MensajeError = "Ocurrió un error inesperado al editar la caja.";
                return View(laCajaAEditar);
            }
        }

        public ActionResult VerSINPE(int idCaja)
        {
            CajaDto laCaja = _obtenerCajaPorIdLN.Obtener(idCaja);
            if (laCaja == null) return HttpNotFound();

            List<SINPESDto> laListaDeSINPES = _obtenerSINPESPorTelefonoCajaLN.Obtener(laCaja.TelefonoSINPE);

            ViewBag.NombreCaja = laCaja.Nombre;
            ViewBag.TelefonoSINPE = laCaja.TelefonoSINPE;
            ViewBag.IdComercio = laCaja.IdComercio;
            ViewBag.IdCaja = idCaja;

            return View(laListaDeSINPES);
        }

        [HttpPost]
        public ActionResult SincronizarSINPE(int idSinpe, int idCaja)
        {
            try
            {
                RespuestaAPIDto respuesta = _sinpeApiLN.SincronizarSinpe(idSinpe);

                if (respuesta.EsValido)
                {
                    TempData["Exito"] = respuesta.Mensaje;
                }
                else
                {
                    TempData["Error"] = respuesta.Mensaje;
                }

                return RedirectToAction("VerSINPE", new { idCaja = idCaja });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al sincronizar el SINPE: " + ex.Message;
                return RedirectToAction("VerSINPE", new { idCaja = idCaja });
            }
        }
    }
}