using MiPrimeraSolucionJMKK.LogicaDeNegocio.Reservaciones;
using System;
using System.Web.Mvc;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reservacion;

// Requiere: POST endpoint para registrar reservación

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = "Empleado,Administrador")]
    public class ReservacionesController : Controller
    {
        private readonly ObtenerReservacionesLN _ln;
        private readonly RegistrarReservacionLN _lnRegistrar;

        public ReservacionesController()
        {
            _ln = new ObtenerReservacionesLN();
            _lnRegistrar = new RegistrarReservacionLN();
        }

        public ActionResult Index(DateTime? fecha, bool? estado)
        {
            try
            {
                var lista = _ln.Obtener(fecha, estado);

                if (lista == null || lista.Count == 0)
                {
                    TempData["MensajeInfo"] = fecha.HasValue || estado.HasValue
                        ? "No existen reservaciones registradas con los filtros seleccionados"
                        : "No existen reservaciones registradas";
                }

                return View(lista);
            }
            catch (Exception ex)
            {
                // Registrar excepcion para diagnostico
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.Log(ex);
                TempData["MensajeError"] = "Error en el sistema por favor intente nuevamente.";
                return View(new System.Collections.Generic.List<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reservacion.ReservacionDto>());
            }
        }

        [HttpGet]
        [Authorize(Roles = "Empleado,Administrador")]
        public ActionResult Registrar()
        {
            return View(new ReservacionRequestDto());
        }

        [HttpPost]
        [Authorize(Roles = "Empleado,Administrador")]
        public JsonResult Registrar(string Fecha, string HoraInicio, string HoraFin, int CantidadPersonas, int IdCliente, int IdMesa, string Observaciones)
        {
            try
            {
                // Log request inputs for debugging
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.LogMessage($"RegistrarReservacion request: Fecha={Fecha}, HoraInicio={HoraInicio}, HoraFin={HoraFin}, Cantidad={CantidadPersonas}, IdCliente={IdCliente}, IdMesa={IdMesa}");
                // Parse inputs
                DateTime fechaParsed;
                if (!DateTime.TryParse(Fecha, out fechaParsed))
                    throw new ArgumentException("Fecha inválida");

                TimeSpan hi, hf;
                if (!TimeSpan.TryParse(HoraInicio, out hi) || !TimeSpan.TryParse(HoraFin, out hf))
                    throw new ArgumentException("Hora inválida");

                var dto = new ReservacionRequestDto
                {
                    Fecha = fechaParsed,
                    HoraInicio = hi,
                    HoraFin = hf,
                    CantidadPersonas = CantidadPersonas,
                    IdCliente = IdCliente,
                    IdMesa = IdMesa,
                    Observaciones = Observaciones
                };

                _lnRegistrar.Registrar(dto);
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.LogMessage($"RegistrarReservacion success: Cliente={IdCliente}, Mesa={IdMesa}, Fecha={fechaParsed.ToShortDateString()}");
                return Json(new { success = true, message = "Reservación registrada correctamente." });
            }
            catch (ArgumentException aex)
            {
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.LogMessage($"RegistrarReservacion argument error: {aex.Message}");
                return Json(new { success = false, message = aex.Message });
            }
            catch (InvalidOperationException iex)
            {
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.LogMessage($"RegistrarReservacion invalid operation: {iex.Message}");
                return Json(new { success = false, message = iex.Message });
            }
            catch (Exception ex)
            {
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.Log(ex);
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.LogMessage($"RegistrarReservacion unexpected error: {ex.Message}");
                return Json(new { success = false, message = "Error en el sistema, por favor intente nuevamente." });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Empleado,Administrador")]
        public JsonResult Cancelar(int id)
        {
            try
            {
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.LogMessage($"CancelarReservacion request: Id={id}");
                var lnCancelar = new MiPrimeraSolucionJMKK.LogicaDeNegocio.Reservaciones.CancelarReservacionLN();
                lnCancelar.Cancelar(id);
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.LogMessage($"CancelarReservacion success: Id={id}");
                return Json(new { success = true, message = "Reservación cancelada correctamente." });
            }
            catch (InvalidOperationException iex)
            {
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.LogMessage($"CancelarReservacion invalid operation: {iex.Message}");
                return Json(new { success = false, message = iex.Message });
            }
            catch (Exception ex)
            {
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.Log(ex);
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.LogMessage($"CancelarReservacion unexpected error: {ex.Message}");
                return Json(new { success = false, message = "Error en el sistema, por favor intente nuevamente." });
            }
        }
    }
}
