using MiPrimeraSolucionJMKK.LogicaDeNegocio.Inventario;
using System;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class InventarioController : Controller
    {
        private readonly ObtenerProductosLN _ln;
        private readonly MiPrimeraSolucionJMKK.LogicaDeNegocio.Inventario.RegistrarProducto.RegistrarProductoLN _registrarLN;

        public InventarioController()
        {
            _ln = new ObtenerProductosLN();
            _registrarLN = new MiPrimeraSolucionJMKK.LogicaDeNegocio.Inventario.RegistrarProducto.RegistrarProductoLN();
        }

        private void CargarCategorias()
        {
            var categorias = _ln.ObtenerCategorias();
            ViewBag.Categorias = new SelectList(categorias);
        }

        public ActionResult Index()
        {
            var lista = _ln.ObtenerTodos();

            CargarCategorias();

            if (lista == null || lista.Count == 0)
            {
                TempData["MensajeInfo"] = "No hay productos disponibles en el inventario";
            }

            return View(lista);
        }

        [HttpPost]
        public ActionResult Buscar(string termino, string categoria, decimal? minCantidad, decimal? maxCantidad)
        {
            CargarCategorias();

            if (!string.IsNullOrWhiteSpace(categoria) || minCantidad.HasValue || maxCantidad.HasValue)
            {
                var listaFiltro = _ln.ObtenerPorFiltro(categoria, minCantidad, maxCantidad);

                if (listaFiltro == null || listaFiltro.Count == 0)
                {
                    TempData["MensajeInfo"] = "No hay productos disponibles en el inventario";
                }

                return View("Index", listaFiltro);
            }

            var lista = _ln.Buscar(termino);

            if (lista == null || lista.Count == 0)
            {
                TempData["MensajeInfo"] = "No se encontraron productos que coincidan con la búsqueda";
            }

            return View("Index", lista);
        }

        // GET: Formulario agregar producto
        public ActionResult AgregarProducto()
        {
            CargarCategorias();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarProducto(MiPrimeraSolucionJMKK.Abstracciones.Modelos.Productos.ProductoDto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    foreach (var key in ModelState.Keys)
                    {
                        var state = ModelState[key];

                        foreach (var err in state.Errors)
                        {
                            MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.LogMessage(
                                $"ModelState error [{key}]: {err.ErrorMessage} {err.Exception?.Message}");
                        }
                    }

                    CargarCategorias();
                    return View(producto);
                }

                bool ok = _registrarLN.Registrar(producto);

                if (ok)
                {
                    TempData["MensajeExito"] = "El producto fue registrado de manera exitosa";
                    return RedirectToAction("Index");
                }

                TempData["MensajeInfo"] = "No se pudo registrar el producto";

                CargarCategorias();
                return View(producto);
            }
            catch (ArgumentException aex)
            {
                TempData["MensajeError"] = aex.Message;

                CargarCategorias();
                return View(producto);
            }
            catch
            {
                TempData["MensajeError"] = "Error en el sistema. Por favor intente nuevamente";

                CargarCategorias();
                return View(producto);
            }
        }
    }
}