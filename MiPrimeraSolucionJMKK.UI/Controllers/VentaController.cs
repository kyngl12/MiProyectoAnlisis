using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Ventas;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ventas;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Ventas;
using MiPrimeraSolucionJMKK.UI.Helpers;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    /// <summary>
    /// Modulo de Ventas (GPV-001 a GPV-005). Toda la logica de negocio
    /// vive en los Flow (LogicaDeNegocio); este controller solo
    /// traduce la solicitud HTTP y el resultado de negocio a la
    /// respuesta HTTP correspondiente.
    /// </summary>
    [Authorize]
    [RoutePrefix("api/ventas")]
    public class VentaController : ApiController
    {
        private readonly IRegistrarVentaLN _registrarVentaLN;
        private readonly IProcesarVentaLN _procesarVentaLN;
        private readonly IAnularVentaLN _anularVentaLN;
        private readonly IObtenerDetalleVentaLN _obtenerDetalleVentaLN;
        private readonly IBuscarProductosLN _buscarProductosLN;

        public VentaController()
        {
            _registrarVentaLN = new RegistrarVentaLN();
            _procesarVentaLN = new ProcesarVentaLN();
            _anularVentaLN = new AnularVentaLN();
            _obtenerDetalleVentaLN = new ObtenerDetalleVentaLN();
            _buscarProductosLN = new BuscarProductosLN();
        }

        /// <summary>
        /// GPV-001: registra una venta nueva con sus productos y cantidades.
        /// </summary>
        [HttpPost]
        [Route("")]
        public HttpResponseMessage Registrar([FromBody] VentaRequestDto venta)
        {
            try
            {
                if (venta == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, new { mensaje = "Debe enviar los datos de la venta." });
                }

                var resultado = _registrarVentaLN.Registrar(venta);

                if (resultado.Exitoso)
                {
                    var response = Request.CreateResponse(HttpStatusCode.Created, new { idVenta = resultado.Data });
                    response.Headers.Location = new Uri(Request.RequestUri, $"api/ventas/{resultado.Data}");
                    return response;
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, new { mensaje = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error en el sistema. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GPV-002: procesa el pago de una venta, descuenta el
        /// inventario y genera el comprobante correspondiente.
        /// </summary>
        [HttpPost]
        [Route("{id:int}/procesar")]
        public HttpResponseMessage Procesar(int id, [FromBody] ProcesarVentaRequestDto request)
        {
            try
            {
                var resultado = _procesarVentaLN.Procesar(id, request);

                if (resultado.Exitoso)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, resultado.Data);
                }

                return Request.CreateResponse(MapearCodigoHttp(resultado.Codigo), new { mensaje = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error en el sistema. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GPV-003: anula una venta mediante borrado logico de estado,
        /// restaurando el inventario si ya habia sido pagada.
        /// </summary>
        [HttpPost]
        [Route("{id:int}/anular")]
        public HttpResponseMessage Anular(int id, [FromBody] AnularVentaRequestDto request)
        {
            try
            {
                var resultado = _anularVentaLN.Anular(id, request);

                if (resultado.Exitoso)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { mensaje = "La venta fue anulada correctamente." });
                }

                return Request.CreateResponse(MapearCodigoHttp(resultado.Codigo), new { mensaje = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error en el sistema. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GPV-005: consulta el detalle completo de una venta (cabecera + lineas + totales).
        /// </summary>
        [HttpGet]
        [Route("{id:int}")]
        public HttpResponseMessage ObtenerDetalle(int id)
        {
            try
            {
                var resultado = _obtenerDetalleVentaLN.ObtenerDetalle(id);

                if (resultado.Exitoso)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, resultado.Data);
                }

                return Request.CreateResponse(MapearCodigoHttp(resultado.Codigo), new { mensaje = resultado.Mensaje });
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error en el sistema. Por favor intente nuevamente." });
            }
        }

        /// <summary>
        /// GPV-004: busca productos activos con stock disponible por
        /// nombre, codigo o categoria (busqueda parcial).
        /// </summary>
        [HttpGet]
        [Route("buscar-productos")]
        public HttpResponseMessage BuscarProductos([FromUri] string texto = null)
        {
            try
            {
                var resultado = _buscarProductosLN.Buscar(texto);
                return Request.CreateResponse(HttpStatusCode.OK, resultado.Data);
            }
            catch (Exception ex)
            {
                LogHelper.Log(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error en el sistema. Por favor intente nuevamente." });
            }
        }

        private static HttpStatusCode MapearCodigoHttp(VentaResultadoCodigo codigo)
        {
            switch (codigo)
            {
                case VentaResultadoCodigo.VentaNoExiste:
                    return HttpStatusCode.NotFound;
                case VentaResultadoCodigo.VentaYaPagada:
                case VentaResultadoCodigo.VentaAnulada:
                case VentaResultadoCodigo.VentaYaAnulada:
                    return HttpStatusCode.Conflict;
                case VentaResultadoCodigo.DatosInvalidos:
                case VentaResultadoCodigo.ProductoNoExiste:
                case VentaResultadoCodigo.CantidadInvalida:
                case VentaResultadoCodigo.StockInsuficiente:
                case VentaResultadoCodigo.SinProductos:
                    return HttpStatusCode.BadRequest;
                default:
                    return HttpStatusCode.InternalServerError;
            }
        }
    }
}
