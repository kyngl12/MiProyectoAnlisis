using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GestionPubRock.AccesoADatos;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Ventas;
using MiPrimeraSolucionJMKK.UI.Helpers;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    /// <summary>
    /// Vista MVC del modulo de Ventas para uso desde el navegador por
    /// Empleado/Administrador. Reutiliza los mismos Flow que el Web API
    /// de Ventas (VentaController).
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

        // Pantalla principal de ventas (busqueda + registro + procesar/anular).
        public ActionResult Index()
        {
            return View();
        }

        // Datos de apoyo para la pantalla: tipos de pago disponibles y el listado
        // inicial de productos, para que la vista no dependa de que el usuario
        // escriba nada antes de poder operar.
        [HttpGet]
        public JsonResult ObtenerDatosIniciales()
        {
            try
            {
                using (var ctx = new Contexto())
                {
                    var tiposPago = ctx.Database
                        .SqlQuery<TipoPagoDto>("SELECT ID_TIPO_PAGO AS IdTipoPago, DESCRIPCION AS Descripcion FROM PUBROCK_TIPO_PAGO_TB WHERE ID_ESTADO = 1")
                        .ToList();

                    var productos = _buscarProductosLN.Buscar(null).Data;

                    return Json(new { success = true, tiposPago, productos }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Json(new { success = false, message = "Error en el sistema, por favor intente nuevamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        // Lista de empleados activos, para asociar la venta sin pedir el id manualmente.
        [HttpGet]
        public JsonResult ObtenerEmpleados()
        {
            try
            {
                using (var ctx = new Contexto())
                {
                    var empleados = ctx.Database.SqlQuery<EmpleadoDto>(
                        "SELECT e.ID_EMPLEADO AS IdEmpleado, u.NOMBRE + ' ' + u.APELLIDO_PATERNO AS Nombre " +
                        "FROM PUBROCK_EMPLEADO_TB e " +
                        "INNER JOIN PUBROCK_USUARIO_TB u ON u.CEDULA = e.CEDULA " +
                        "WHERE e.ID_ESTADO = 1 ORDER BY Nombre").ToList();

                    return Json(new { success = true, data = empleados }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Json(new { success = false, message = "Error en el sistema, por favor intente nuevamente." }, JsonRequestBehavior.AllowGet);
            }
        }

        private class EmpleadoDto
        {
            public int IdEmpleado { get; set; }
            public string Nombre { get; set; }
        }

        // Busca productos activos con stock disponible.
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

        private class TipoPagoDto
        {
            public int IdTipoPago { get; set; }
            public string Descripcion { get; set; }
        }

        // Registra una venta nueva con su detalle de productos.
        [HttpPost]
        public JsonResult Registrar(VentaRequestDto venta)
        {
            try
            {
                if (venta == null || venta.Detalle == null || venta.Detalle.Count == 0)
                {
                    return Json(new { success = false, message = "Debe enviar los datos de la venta con al menos un producto." });
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

        // Procesa el pago de una venta y genera el comprobante.
        [HttpPost]
        public JsonResult Procesar(int? idVenta, ProcesarVentaRequestDto request)
        {
            try
            {
                if (!idVenta.HasValue || idVenta.Value <= 0 || request == null || request.IdTipoPago <= 0)
                {
                    return Json(new { success = false, message = "Debe indicar la venta y el tipo de pago." });
                }

                var resultado = _procesarVentaLN.Procesar(idVenta.Value, request);

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

        // Anula una venta (borrado logico), restaurando inventario si aplica.
        [HttpPost]
        public JsonResult Anular(int? idVenta, AnularVentaRequestDto request)
        {
            try
            {
                if (!idVenta.HasValue || idVenta.Value <= 0)
                {
                    return Json(new { success = false, message = "Debe indicar la venta a anular." });
                }

                var resultado = _anularVentaLN.Anular(idVenta.Value, request);

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

        // Consulta el detalle completo de una venta.
        [HttpGet]
        public JsonResult ObtenerDetalle(int? idVenta)
        {
            try
            {
                if (!idVenta.HasValue || idVenta.Value <= 0)
                {
                    return Json(new { success = false, message = "Debe indicar la venta a consultar." }, JsonRequestBehavior.AllowGet);
                }

                var resultado = _obtenerDetalleVentaLN.ObtenerDetalle(idVenta.Value);

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
