using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Cajas.ObtenerCajasPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Cajas;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Cajas.ObtenerCajasPorComercio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    public class CajaAPIController : ApiController
    {
        private IObtenerCajasPorComercioLN _logica;

        public CajaAPIController()
        {
            _logica = new ObtenerCajasPorComercioLN();
        }

        // GET api/CajaAPI
        [Authorize]
        public IEnumerable<CajaDto> GetByComercio(int idComercio)
        {
            return _logica.Obtener(idComercio);
        }
        // GET api/CajaAPI/5
        [Authorize]
        public string Get(int id)
        {
            return "Caja " + id;
        }
        // POST api/CajaAPI
        [Authorize]
        public void Post([FromBody] CajaDto modelo)
        {
        }

        // PUT api/CajaAPI/5
        [Authorize]
        public void Put(int id, [FromBody] CajaDto modelo)
        {
        }

        // DELETE api/CajaAPI/5
        [Authorize]
        public void Delete(int id)
        {
        }
    }
}