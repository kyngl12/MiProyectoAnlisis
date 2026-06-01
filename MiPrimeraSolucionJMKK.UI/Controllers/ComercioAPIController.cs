using Microsoft.IdentityModel.Tokens;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Comercios.ObtenerTodosLosComercios;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorComercio;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Comercios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Comercios.ObtenerTodosLosComercios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.ConfiguracionComercio.ObtenerConfiguracionPorComercio;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    public class ComercioAPIController : ApiController
    {
        private IObtenerTodosLosComerciosLN _logica;
        private IObtenerConfiguracionPorComercioLN _configuracionLogica;

        public ComercioAPIController()
        {
            _logica = new ObtenerTodosLosComerciosLN();
            _configuracionLogica = new ObtenerConfiguracionPorComercioLN();
        }

        // POST api/ComercioAPI
        [System.Web.Http.HttpPost]
        public IHttpActionResult Post([FromBody] ComercioDto modelo)
        {
            if (modelo == null)
                return Unauthorized();

            var comercio = _logica.Obtener()
                .FirstOrDefault(x =>
                    x.IdComercio == modelo.IdComercio);

            if (comercio == null)
                return Unauthorized();

            if (comercio.Estado == false)
                return Unauthorized();

            var configuraciones = _configuracionLogica.Obtener(modelo.IdComercio);
            var configuracion = configuraciones.FirstOrDefault(c => c.Estado);

            if (configuracion == null)
                return Content(HttpStatusCode.Unauthorized, new { mensaje = "El comercio no tiene configuración activa" });

            if (configuracion.TipoConfiguracion == 1)
                return Content(HttpStatusCode.Unauthorized, new { mensaje = "El comercio solo está configurado para usar la plataforma interna" });

            var issuer = "miAplicacion";
            var audience = "miAplicacion";
            var secret =
                Encoding.UTF8.GetBytes(
                    "clave_super_secreta_123");

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.Name,
                    comercio.Nombre),

                new Claim(
                    ClaimTypes.NameIdentifier,
                    comercio.IdComercio.ToString())
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials:
                    new SigningCredentials(
                        new SymmetricSecurityKey(secret),
                        SecurityAlgorithms.HmacSha256)
            );

            var tokenString =
                new JwtSecurityTokenHandler()
                .WriteToken(token);

            return Ok(new { token = tokenString });
        }

        [System.Web.Http.Authorize]
        public IHttpActionResult Get()
        {
            return Ok("Acceso autorizado");
        }
    }
}
