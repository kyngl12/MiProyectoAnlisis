using System;
using System.Web.Mvc;
using GestionPubRock.LogicaDeNegocio.Bitacora.BitacoraContable;
using MiPrimeraSolucionJMKK.Abstacciones.LogicaDeNegocio.Bitacora.BitacoraContable;
using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;
using MiPrimeraSolucionJMKK.UI.Helpers;

namespace GestionPubRock.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class BitacoraContableController : Controller
    {
        private readonly IObtenerBitacoraContableLN _obtenerBitacoraContableLN;

        public BitacoraContableController()
        {
            _obtenerBitacoraContableLN = new ObtenerBitacoraContableLN();
        }

        // GET: BitacoraContable
        public ActionResult Index(string cedula, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var filtro = new BitacoraContableFiltroDto
            {
                Cedula = cedula,
                FechaInicio = fechaInicio,
                FechaFin = fechaFin
            };

            var lista = _obtenerBitacoraContableLN.Obtener(filtro);

            if (lista == null || lista.Count == 0)
            {
                TempData["MensajeInfo"] = "No se encontraron movimientos contables para los filtros seleccionados.";
            }

            ViewBag.Cedula = cedula;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;

            return View(lista);
        }

        // GET: BitacoraContable/DescargarPdf
        public ActionResult DescargarPdf(string cedula, DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                var filtro = new BitacoraContableFiltroDto
                {
                    Cedula = cedula,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin
                };

                var lista = _obtenerBitacoraContableLN.Obtener(filtro);

                var pdf = new SimplePdfBuilder("Bitácora Contable - PubRock");
                pdf.AgregarSeparador();

                foreach (var evento in lista)
                {
                    pdf.AgregarLinea(evento.FechaHora.ToString("dd/MM/yyyy HH:mm") + " | " +
                        (evento.NombreUsuario ?? evento.Cedula) + " | " +
                        evento.AccionRealizada);
                    pdf.AgregarLinea("   " + evento.Descripcion);
                }

                byte[] archivo = pdf.Generar();
                return File(archivo, "application/pdf", "BitacoraContable_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                TempData["MensajeError"] = "Error en el sistema. Favor intente de nuevo.";
                return RedirectToAction("Index");
            }
        }
    }
}
