using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.EditarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.InactivarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerTodosLosTipoUsuarios;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.EditarUsuario;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.InactivarUsuario;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.ObtenerTodosLosTipoUsuarios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class UsuariosController : Controller
    {
        private IObtenerTodosLosUsuariosLN _obtenerTodosLosUsuariosLN;
        private IRegistrarUsuarioLN _registrarUsuarioLN;
        private IEditarUsuarioLN _editarUsuarioLN;
        private IObtenerTodosLosTiposDeUsuarioLN _obtenerTiposDeUsuarioLN;
        private IInactivarUsuarioLN _inactivarUsuarioLN;

        public UsuariosController()
        {
            _obtenerTodosLosUsuariosLN = new ObtenerTodosLosUsuariosLN();
            _registrarUsuarioLN = new RegistrarUsuarioLN();
            _editarUsuarioLN = new EditarUsuarioLN();
            _obtenerTiposDeUsuarioLN = new ObtenerTodosLosTiposDeUsuarioLN();
            _inactivarUsuarioLN = new InactivarUsuarioLN();
        }

        public ActionResult Index() => RedirectToAction("ObtenerTodosLosUsuarios");

        public ActionResult ObtenerTodosLosUsuarios()
        {
            List<UsuarioDto> laLista = _obtenerTodosLosUsuariosLN.Obtener();
            return View(laLista);
        }

        public ActionResult DetallesDelUsuario(string cedula)
        {
            UsuarioDto elUsuario = _obtenerUsuarioPorCedulaLN.Obtener(cedula);
            return View(elUsuario);
        }

        // GET: Formulario agregar
        public ActionResult AgregarUsuario()
        {
            CargarTiposDeUsuario();
            return View(new UsuarioDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarUsuario(UsuarioDto elUsuarioAGuardar)
        {
            // Controller solo valida formato (Escenario 3 y 4 de formato básico)
            if (!ModelState.IsValid)
            {
                CargarTiposDeUsuario();
                return View(elUsuarioAGuardar);
            }

            try
            {
                bool seAgrego = _registrarUsuarioLN.Registrar(elUsuarioAGuardar);

                if (seAgrego)
                {
   
                    TempData["MensajeExito"] = "El usuario se registró de manera exitosa.";
                    return RedirectToAction("ObtenerTodosLosUsuarios");
                }

                TempData["MensajeError"] = "El usuario ya se encuentra registrado.";
                CargarTiposDeUsuario();
                return View(elUsuarioAGuardar);
            }
            catch
            {
                TempData["MensajeError"] = "Ocurrió un error inesperado al registrar el usuario.";
                CargarTiposDeUsuario();
                return View(elUsuarioAGuardar);
            }
        }

        // GET: Formulario editar
        public ActionResult EditarUsuario(string cedula)
        {
            UsuarioDto elUsuario = _obtenerUsuarioPorCedulaLN.Obtener(cedula);
            CargarTiposDeUsuario();
            return View(elUsuario);
        }

        // POST: Actualizar usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(string cedula, UsuarioDto elUsuarioParaActualizar)
        {
            if (!ModelState.IsValid)
            {
                CargarTiposDeUsuario();
                return View(elUsuarioParaActualizar);
            }

            try
            {
                bool seActualizo = _editarUsuarioLN.Editar(elUsuarioParaActualizar);

                if (seActualizo)
                {
                    TempData["MensajeExito"] = "El usuario se actualizó de manera exitosa.";
                    return RedirectToAction("ObtenerTodosLosUsuarios");
                }

                TempData["MensajeError"] = "Error al actualizar. Verifique los datos ingresados.";
                CargarTiposDeUsuario();
                return View(elUsuarioParaActualizar);
            }
            catch
            {
                TempData["MensajeError"] = "Ocurrió un error inesperado al actualizar el usuario.";
                CargarTiposDeUsuario();
                return View(elUsuarioParaActualizar);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InactivarUsuario(string cedula)
        {
            try
            {
                int resultado = _inactivarUsuarioLN.Inactivar(cedula);

                if (resultado > 0)
                {
                    TempData["MensajeExito"] = "El usuario fue inactivado exitosamente.";
                    return RedirectToAction("ObtenerTodosLosUsuarios");
                }

                if (resultado == -2)
                {
                    TempData["MensajeError"] = "Error al inactivar. El usuario ya se encuentra inactivo.";
                    return RedirectToAction("ObtenerTodosLosUsuarios");
                }

                TempData["MensajeError"] = "Error al inactivar. El usuario no se encuentra registrado en el sistema.";
                return RedirectToAction("ObtenerTodosLosUsuarios");
            }
            catch
            {
                TempData["MensajeError"] = "Ocurrió un error inesperado al inactivar el usuario.";
                return RedirectToAction("ObtenerTodosLosUsuarios");
            }
        }

        private void CargarTiposDeUsuario()
        {
            var tipos = _obtenerTiposDeUsuarioLN.ObtenerTodos();
            ViewBag.TiposDeUsuario = new SelectList(tipos, "IdTipoUsuario", "Descripcion");
        }
    }
}
