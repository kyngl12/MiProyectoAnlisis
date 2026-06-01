using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MiPrimeraSolucionJMKK.Abstacciones.Modelos.Bitacora;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Bitacora.ObtenerBitacora;
using MiPrimeraSolucionJMKK.AccesoADatos.Bitacora.ObtenerBitacora;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = ("Administrador"))]
    public class BitacoraController : Controller
    {
        private readonly ObtenerBitacoraLN _bitacoraLN;

        public BitacoraController()
        {
            string connectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["Contexto"].ConnectionString;

            var bitacoraAD = new ObtenerBitacoraAD(connectionString);
            _bitacoraLN = new ObtenerBitacoraLN(bitacoraAD);
        }

        // GET: Bitacora
        public ActionResult Index()
        {
            List<BitacoraEventoDto> lista = _bitacoraLN.Ejecutar();
            return View(lista);
        }
    }
}