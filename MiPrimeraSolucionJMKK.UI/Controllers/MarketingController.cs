using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class MarketingController : Controller
    {
        // GET: Marketing
        public ActionResult Index()
        {
            // Carga contenido estático por ahora; si se requiere, enlazar a LN/AD para contenido dinámico
            return View();
        }
    }
}
