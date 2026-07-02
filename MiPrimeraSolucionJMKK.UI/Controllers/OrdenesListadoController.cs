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

        // Búsqueda y filtros con paginación simple
        [HttpGet]
        public ActionResult Search(string termino, System.DateTime? fecha, int? estado, int page = 1, int pageSize = 20)
        {
            var lista = _ln.ObtenerTodos();

            if (!string.IsNullOrWhiteSpace(termino))
                lista = lista.FindAll(x => (x.Observaciones ?? "").IndexOf(termino, System.StringComparison.OrdinalIgnoreCase) >= 0 || x.IdOrden.ToString() == termino);

            if (fecha.HasValue)
                lista = lista.FindAll(x => x.FechaOrden.Date == fecha.Value.Date);

            if (estado.HasValue)
                lista = lista.FindAll(x => x.IdEstadoOrden == estado.Value);

            // ordenamiento por fecha descendente por defecto
            lista.Sort((a, b) => b.FechaOrden.CompareTo(a.FechaOrden));

            // paginación simple
            var total = lista.Count;
            var paged = lista.GetRange((page - 1) * pageSize, System.Math.Min(pageSize, System.Math.Max(0, total - (page - 1) * pageSize)));

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.Total = total;

            return View("Index", paged);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eliminar(int id)
        {
            try
            {
                var ctx = new GestionPubRock.AccesoADatos.Contexto();
                // Soft delete: marcar estado como 'Inactivo' consultando tabla de estados
                ctx.Database.ExecuteSqlCommand("UPDATE PUBROCK_ORDEN_TB SET ID_ESTADO = (SELECT ID_ESTADO FROM PUBROCK_ESTADO_TB WHERE DESCRIPCION = 'Inactivo') WHERE ID_ORDEN = @p0", id);
                TempData["MensajeExitoOrden"] = "Orden eliminada correctamente";
            }
            catch
            {
                TempData["MensajeError"] = "Ocurrió un error al eliminar la orden";
            }

            return RedirectToAction("Index");
        }
    }
}
