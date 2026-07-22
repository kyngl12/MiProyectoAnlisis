using GestionPubRock.LogicaDeNegocio.Inventario;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Inventario;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = "Administrador, Cliente")]
    public class InventarioController : Controller
    {

        private readonly ObtenerProductosLN _ln;
        private readonly MiPrimeraSolucionJMKK.LogicaDeNegocio.Inventario.RegistrarProducto.RegistrarProductoLN _registrarLN;
        private readonly EditarProductoLN _editarLN;

        public InventarioController()
        {
            _ln = new ObtenerProductosLN();
            _registrarLN = new MiPrimeraSolucionJMKK.LogicaDeNegocio.Inventario.RegistrarProducto.RegistrarProductoLN();
            _editarLN = new EditarProductoLN();
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

        // Editar
        public ActionResult EditarProducto(int id)
        {
            try
            {
                var producto = _ln.ObtenerPorId(id);

                if (producto == null)
                {
                    TempData["MensajeError"] = "El producto no existe";
                    return RedirectToAction("Index");
                }

                CargarCategorias();
                return View(producto);
            }
            catch
            {
                TempData["MensajeError"] = "Error al cargar el producto";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProducto(MiPrimeraSolucionJMKK.Abstracciones.Modelos.Productos.ProductoDto producto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    CargarCategorias();
                    return View(producto);
                }

                bool ok = _editarLN.Editar(producto);

                if (ok)
                {
                    TempData["MensajeExito"] = "El producto fue actualizado correctamente";
                    return RedirectToAction("Index");
                }

                TempData["MensajeInfo"] = "No se pudo actualizar el producto";

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

        // Eliminar
        [HttpPost]
        public ActionResult EliminarProducto(int id)
        {
            var ln = new EliminarProductoLN();
            var resultado = ln.Eliminar(id);

            if (resultado.Contains("correctamente"))
            {
                TempData["MensajeExito"] = resultado;
            }
            else
            {
                TempData["MensajeError"] = resultado;
            }

            return RedirectToAction("Index");
        }

        // Movimiento bitacora
        [HttpPost]
        public ActionResult RegistrarMovimiento(int idProducto, int cantidad, string tipo, string motivo)
        {
            var ln = new MovimientoInventarioLN();
            var resultado = ln.RegistrarMovimiento(idProducto, cantidad, tipo, motivo);

            if (resultado.Contains("correctamente"))
            {
                TempData["MensajeExito"] = resultado;
            }
            else if (resultado.Contains("Stock bajo"))
            {
                TempData["MensajeInfo"] = resultado;
            }
            else
            {
                TempData["MensajeError"] = resultado;
            }

            return RedirectToAction("Index");
        }

        // ==========================================
        // VISTA CLIENTE: CATÁLOGO DE PRODUCTOS
        // ==========================================

        [Authorize(Roles = "Cliente")]
        public ActionResult Catalogo()
        {
            CargarCategorias();

            var lista = _ln.ObtenerTodos();

            if (lista == null || !lista.Any())
            {
                TempData["MensajeInfo"] = "No hay productos disponibles en el catálogo.";
            }

            return View(lista);
        }

        [HttpPost]
        [Authorize(Roles = "Cliente")]
        public ActionResult Catalogo(string categoria)
        {
            CargarCategorias();

            // Si se selecciona la opción vacía o "-- Todas las categorías --", se recargan todos
            if (string.IsNullOrWhiteSpace(categoria))
            {
                return RedirectToAction("Catalogo");
            }

            var lista = _ln.ObtenerPorFiltro(categoria, null, null);

            if (lista == null || !lista.Any())
            {
                TempData["MensajeInfo"] = "No se encontraron productos para esa categoría.";
            }

            return View(lista);
        }
    }
}