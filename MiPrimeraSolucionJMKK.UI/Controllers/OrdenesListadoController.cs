using System.Web.Mvc;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Ordenes;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class OrdenesListadoController : Controller
    {
        private readonly ObtenerOrdenesLN _ln;

        public OrdenesListadoController()
        {
            _ln = new ObtenerOrdenesLN();
        }

        public ActionResult Index()
        {
            var lista = _ln.ObtenerTodos();
            return View(lista);
        }
    }
}
