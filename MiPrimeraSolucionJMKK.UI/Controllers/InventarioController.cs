using GestionPubRock.LogicaDeNegocio.Inventario;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Inventario;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

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

        // GET: Inventario/ReporteInventario
        public ActionResult ReporteInventario(int? idCategoria, int? idProveedor, int? idEstado)
        {
            try
            {
                using (var ctx = new GestionPubRock.AccesoADatos.Contexto())
                {
                    var p1 = new System.Data.SqlClient.SqlParameter("@IdCategoria", (object)idCategoria ?? DBNull.Value);
                    var p2 = new System.Data.SqlClient.SqlParameter("@IdProveedor", (object)idProveedor ?? DBNull.Value);
                    var p3 = new System.Data.SqlClient.SqlParameter("@IdEstado", (object)idEstado ?? DBNull.Value);

                    var sql = "EXEC SP_PUBROCK_REPORTE_INVENTARIO @IdCategoria, @IdProveedor, @IdEstado";
                    var rows = ctx.Database.SqlQuery<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes.ReporteInventarioRowDto>(sql, p1, p2, p3).ToList();

                    ViewBag.Inventario = rows ?? new List<MiPrimeraSolucionJMKK.Abstracciones.Modelos.Reportes.ReporteInventarioRowDto>();
                    return View();
                }
            }
            catch (Exception ex)
            {
                ViewBag.MensajeError = ex.Message;
                ViewBag.Inventario = new List<object>();
                return View();
            }
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