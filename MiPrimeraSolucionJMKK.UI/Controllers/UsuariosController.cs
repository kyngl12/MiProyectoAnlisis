using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MiPrimeraSolucionJMKK.Abstracciones.Modelos.Usuarios;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.Abstracciones.LogicaDeNegocio.Usuarios.EditarUsuario;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.ObtenerTodosLosUsuarios;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.ObtenerUsuarioPorId;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.RegistrarUsuario;
using MiPrimeraSolucionJMKK.LogicaDeNegocio.Usuarios.EditarUsuario;

namespace MiPrimeraSolucionJMKK.UI.Controllers
{
    [Authorize(Roles = ("Administrador"))]
    public class UsuariosController : Controller
    {
        private IObtenerTodosLosUsuariosLN _obtenerTodosLosUsuariosLN;
        private IObtenerUsuarioPorIdLN _obtenerUsuarioPorIdLN;
        private IRegistrarUsuarioLN _registrarUsuarioLN;
        private IEditarUsuarioLN _editarUsuarioLN;

        public UsuariosController()
        {
            _obtenerTodosLosUsuariosLN = new ObtenerTodosLosUsuariosLN();
            _obtenerUsuarioPorIdLN = new ObtenerUsuarioPorIdLN();
            _registrarUsuarioLN = new RegistrarUsuarioLN();
            _editarUsuarioLN = new EditarUsuarioLN();
        }

        // GET: Index
        public ActionResult Index()
        {
            return RedirectToAction("ObtenerTodosLosUsuarios");
        }

        // GET: Listar todos los usuarios
        public ActionResult ObtenerTodosLosUsuarios()
        {
            List<UsuarioDto> laListaDeUsuarios = _obtenerTodosLosUsuariosLN.Obtener();
            return View(laListaDeUsuarios);
        }

        // GET: Detalles del usuario
        public ActionResult DetallesDelUsuario(int id)
        {
            UsuarioDto elUsuario = _obtenerUsuarioPorIdLN.Obtener(id);
            return View(elUsuario);
        }

        // GET: Formulario para agregar usuario
        public ActionResult AgregarUsuario()
        {
            return View(new UsuarioDto());
        }

        // POST: Registrar usuario
        [HttpPost]
        public ActionResult AgregarUsuario(UsuarioDto elUsuarioAGuardar)
        {
            try
            {
                bool seAgrego = _registrarUsuarioLN.Registrar(elUsuarioAGuardar);

                if (seAgrego)
                    return RedirectToAction("ObtenerTodosLosUsuarios");

                TempData["Mensaje"] = "Error al agregar. Verifique los datos o si la identificación ya existe.";
                return RedirectToAction("AgregarUsuario");
            }
            catch
            {
                TempData["Mensaje"] = "Ocurrió un error inesperado al registrar el usuario.";
                return RedirectToAction("AgregarUsuario");
            }
        }

        // GET: Formulario para editar usuario
        public ActionResult EditarUsuario(int id)
        {
            UsuarioDto elUsuario = _obtenerUsuarioPorIdLN.Obtener(id);
            return View(elUsuario);
        }

        // POST: Actualizar usuario
        [HttpPost]
        public ActionResult EditarUsuario(int id, UsuarioDto elUsuarioParaActualizar)
        {
            try
            {
                bool seActualizo = _editarUsuarioLN.Editar(elUsuarioParaActualizar);

                if (seActualizo)
                    return RedirectToAction("ObtenerTodosLosUsuarios");

                TempData["Mensaje"] = "Error al actualizar. Verifique los datos o si la identificación ya existe.";
                return RedirectToAction("EditarUsuario", new { id = id });
            }
            catch
            {
                TempData["Mensaje"] = "Ocurrió un error inesperado al actualizar el usuario.";
                return RedirectToAction("EditarUsuario", new { id = id });
            }
        }
    }
}
