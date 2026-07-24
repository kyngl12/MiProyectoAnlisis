using GestionPubRock.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Contabilidad;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Contabilidad;
using MiPrimeraSolucionJMKK.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GestionPubRock.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EgresosController : Controller
    {
        private readonly IRegistrarEgresoLN _registrarEgresoLN;
        private readonly IObtenerEgresosLN _obtenerEgresosLN;

        public EgresosController()
        {
            _registrarEgresoLN = new RegistrarEgresoLN();
            _obtenerEgresosLN = new ObtenerEgresosLN();
        }

        private string ObtenerCedulaActual()
        {
            // Intenta obtener la cédula de la sesión
            string cedula = Session["Cedula"] as string;

            // Si la sesión no tiene la cédula, intenta obtenerla de los claims
            if (string.IsNullOrWhiteSpace(cedula) && User?.Identity?.IsAuthenticated == true)
            {
                cedula = User.Identity.Name;
            }

            return cedula;
        }

        // GET: Egresos
        public ActionResult Index()
        {
            var lista = _obtenerEgresosLN.Obtener(null, null);
            return View(lista);
        }

        // GET: Egresos/Agregar
        public ActionResult Agregar()
        {
            var egreso = new EgresoDto
            {
                Fecha = DateTime.Now.Date,
                Empleados = ObtenerListaEmpleados()
            };
            return View(egreso);
        }

        private List<EmpleadoDto> ObtenerListaEmpleados()
        {
            var contexto = new GestionPubRock.AccesoADatos.Contexto();
            return contexto.Database.SqlQuery<EmpleadoDto>(
                @"SELECT e.ID_EMPLEADO AS IdEmpleado, e.CEDULA AS Cedula, 
                         (u.NOMBRE + ' ' + u.APELLIDO_PATERNO) AS NombreCompleto
                  FROM PUBROCK_EMPLEADO_TB e
                  LEFT JOIN PUBROCK_USUARIO_TB u ON e.CEDULA = u.CEDULA
                  WHERE e.ID_ESTADO = 1
                  ORDER BY ISNULL(u.NOMBRE, e.CEDULA)"
            ).ToList();
        }

        // POST: Egresos/Agregar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar(EgresoDto egreso)
        {
            // Escenario 2: campos obligatorios en blanco (resaltados por Data Annotations).
            if (!ModelState.IsValid)
            {
                egreso.Empleados = ObtenerListaEmpleados();
                return View(egreso);
            }

            try
            {
                egreso.RegistradoPor = ObtenerCedulaActual();

                bool ok = _registrarEgresoLN.Registrar(egreso);

                if (ok)
                {
                    TempData["MensajeExito"] = "El gasto se registró de manera exitosa.";
                    return RedirectToAction("Index");
                }

                egreso.Empleados = ObtenerListaEmpleados();
                TempData["MensajeError"] = "Error en el sistema. Favor intente de nuevo.";
                return View(egreso);
            }
            catch (Exception ex)
            {
                egreso.Empleados = ObtenerListaEmpleados();
                LogHelper.Log(ex);
                TempData["MensajeError"] = ex.Message;
                return View(egreso);
            }
        }
    }
}
