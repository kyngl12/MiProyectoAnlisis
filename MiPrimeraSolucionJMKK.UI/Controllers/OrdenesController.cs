using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Ordenes;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Ordenes.RegistrarOrden;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class OrdenesController : Controller
    {
        private readonly RegistrarOrdenLN _ln;

        public OrdenesController()
        {
            _ln = new RegistrarOrdenLN();
        }

        public ActionResult Crear()
        {
            CargarListasOrdenes();
            return View(new OrdenDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(OrdenDto orden)
        {
            try
            {
                int id = _ln.Registrar(orden);
                if (id > 0)
                {
                    TempData["MensajeExitoOrden"] = "La orden fue creada de manera exitosa";
                    return RedirectToAction("Index", "OrdenesListado");
                }

                TempData["MensajeError"] = "No se pudo crear la orden";
                CargarListasOrdenes();
                return View(orden);
            }
            catch (ArgumentException aex)
            {
                TempData["MensajeError"] = aex.Message;
                CargarListasOrdenes();
                return View(orden);
            }
            catch (Exception ex)
            {
                // Log para diagnostico
                MiPrimeraSolucionJMKK.UI.Helpers.LogHelper.Log(ex);
                TempData["MensajeError"] = "Error en el sistema. Por favor intente nuevamente";
                CargarListasOrdenes();
                return View(orden);
            }
        }

        private void CargarListasOrdenes()
        {
            // productos
            System.Collections.Generic.List<ProductDtoSimple> listProd = null;
            try
            {
                var ctx = new GestionPubRock.AccesoADatos.Contexto();
                listProd = ctx.Database.SqlQuery<ProductDtoSimple>("SELECT ID_PRODUCTO AS Id, NOMBRE_PRODUCTO AS Nombre FROM PUBROCK_PRODUCTO_TB").ToList();
            }
            catch { listProd = new System.Collections.Generic.List<ProductDtoSimple>(); }

            ViewBag.Productos = new System.Web.Mvc.SelectList(listProd, "Id", "Nombre");

            // proveedores
            var proveedores = new System.Collections.Generic.List<System.Web.Mvc.SelectListItem>();
            try
            {
                var ctx = new GestionPubRock.AccesoADatos.Contexto();
                var sqlProv = @"SELECT P.ID_PROVEEDOR AS Id, (U.NOMBRE + ' ' + ISNULL(U.APELLIDO_PATERNO,'')) AS Nombre
                                FROM PUBROCK_PROVEEDOR_TB P
                                INNER JOIN PUBROCK_USUARIO_TB U ON P.CEDULA = U.CEDULA";
                var listProv = ctx.Database.SqlQuery<ProveedorDto>(sqlProv).ToList();
                foreach (var p in listProv)
                    proveedores.Add(new System.Web.Mvc.SelectListItem { Value = p.Id.ToString(), Text = p.Nombre });
            }
            catch { }

            ViewBag.Proveedores = proveedores;
        }

    internal class ProveedorDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
    internal class ProductDtoSimple
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
    }
}
