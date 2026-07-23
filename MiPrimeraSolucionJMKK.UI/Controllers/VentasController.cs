using System;
using System.Web.Mvc;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Ventas;
using MiPrimeraSolucionJMKK.UI.Helpers;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    /// <summary>
    /// Vista MVC del modulo de Ventas (GPV-001 a GPV-006) para uso
    /// desde el navegador por Empleado/Administrador. Reutiliza los
    /// mismos Flow que el Web API de Ventas (VentaController).
    /// </summary>
    [Authorize(Roles = "Empleado,Administrador")]
    public class VentasController : Controller
    {
        private readonly IRegistrarVentaLN _registrarVentaLN;
        private readonly IProcesarVentaLN _procesarVentaLN;
        private readonly IAnularVentaLN _anularVentaLN;
        private readonly IObtenerDetalleVentaLN _obtenerDetalleVentaLN;
        private readonly IBuscarProductosLN _buscarProductosLN;

        public VentasController()
        {
            _registrarVentaLN = new RegistrarVentaLN();
            _procesarVentaLN = new ProcesarVentaLN();
            _anularVentaLN = new AnularVentaLN();
            _obtenerDetalleVentaLN = new ObtenerDetalleVentaLN();
            _buscarProductosLN = new BuscarProductosLN();
        }

        // GPV-004: pantalla principal de ventas (busqueda + registro + procesar/anular).
        public ActionResult Index()
        {
            return View();
        }

        // GPV-004: busca productos activos con stock disponible.
        [HttpGet]
        public JsonResult BuscarProductos(string texto)
        {
            try
            {
                var resultado = _buscarProductosLN.Buscar(texto);
                return Json(new { success = true, data = resultado.Data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Json(new { success = false, message = "Error en el sistema, por favor intente nuevamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        // GPV-001: registra una venta nueva con su detalle de productos.
        [HttpPost]
        public JsonResult Registrar(VentaRequestDto venta)
        {
            try
            {
                if (venta == null)
                {
                    return Json(new { success = false, message = "Debe enviar los datos de la venta." });
                }

                var resultado = _registrarVentaLN.Registrar(venta);

                if (!resultado.Exitoso)
                {
                    return Json(new { success = false, message = resultado.Mensaje });
                }

                return Json(new { success = true, message = "Venta registrada correctamente.", idVenta = resultado.Data });
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Json(new { success = false, message = "Error en el sistema, por favor intente nuevamente." });
            }
        }

        // GPV-002: procesa el pago de una venta y genera el comprobante.
        [HttpPost]
        public JsonResult Procesar(int idVenta, ProcesarVentaRequestDto request)
        {
            try
            {
                var resultado = _procesarVentaLN.Procesar(idVenta, request);

                if (!resultado.Exitoso)
                {
                    return Json(new { success = false, message = resultado.Mensaje });
                }

                return Json(new { success = true, message = "Venta procesada correctamente.", comprobante = resultado.Data });
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Json(new { success = false, message = "Error en el sistema, por favor intente nuevamente." });
            }
        }

        // GPV-003: anula una venta (borrado logico), restaurando inventario si aplica.
        [HttpPost]
        public JsonResult Anular(int idVenta, AnularVentaRequestDto request)
        {
            try
            {
                var resultado = _anularVentaLN.Anular(idVenta, request);

                if (!resultado.Exitoso)
                {
                    return Json(new { success = false, message = resultado.Mensaje });
                }

                return Json(new { success = true, message = "La venta fue anulada correctamente." });
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Json(new { success = false, message = "Error en el sistema, por favor intente nuevamente." });
            }
        }

        // GPV-005/GPV-006: consulta el detalle completo de una venta.
        [HttpGet]
        public JsonResult ObtenerDetalle(int idVenta)
        {
            try
            {
                var resultado = _obtenerDetalleVentaLN.ObtenerDetalle(idVenta);

                if (!resultado.Exitoso)
                {
                    return Json(new { success = false, message = resultado.Mensaje }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = true, data = resultado.Data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Json(new { success = false, message = "Error en el sistema, por favor intente nuevamente." }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
