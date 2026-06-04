using MiPrimeraSolucionJMKK.LogicaDeNegocio.Reservaciones;
using System;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = "Empleado,Administrador")]
    public class ReservacionesController : Controller
    {
        private readonly ObtenerReservacionesLN _ln;

        public ReservacionesController()
        {
            _ln = new ObtenerReservacionesLN();
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
    }
}
