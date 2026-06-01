using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.SINPESApi;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPES;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.SINPESApi;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.SINPESApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/sinpe")]
    public class SINPESApiController : ApiController
    {
        private ISINPESApiLN _sinpeApiLN;

        public SINPESApiController()
        {
            _sinpeApiLN = new SinpeApiLN();
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("consultar")]
        public IHttpActionResult ConsultarSinpe(string telefonoCaja)
        {
            if (string.IsNullOrWhiteSpace(telefonoCaja))
            {
                return BadRequest("Debe enviar el teléfono de la caja.");
            }

            List<SINPESDto> lista = _sinpeApiLN.ConsultarSinpesPorTelefonoCaja(telefonoCaja);

            return Ok(lista);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("sincronizar")]
        public IHttpActionResult SincronizarSinpe(SincronizarSINPEDto modelo)
        {
            if (modelo == null || modelo.IdSinpe <= 0)
            {
                return BadRequest("IdSinpe inválido.");
            }

            RespuestaAPIDto respuesta = _sinpeApiLN.SincronizarSinpe(modelo.IdSinpe);

            return Ok(respuesta);
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("recibir")]
        public IHttpActionResult RecibirSinpe(SINPESDto modelo)
        {
            if (modelo == null)
            {
                return BadRequest("Datos inválidos.");
            }

            RespuestaAPIDto respuesta = _sinpeApiLN.RecibirSinpe(modelo);

            return Ok(respuesta);
        }
    }
}