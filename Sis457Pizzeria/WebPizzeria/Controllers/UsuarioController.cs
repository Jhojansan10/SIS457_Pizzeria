using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CadPizzeria;
using ClnPizzeria;

namespace WebPizzeria.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index(string filtro)
        {
            var lista = UsuarioCln.Listar(filtro ?? "");
            ViewBag.Filtro = filtro;
            return View(lista);
        }

        public IActionResult Crear()
        {
            ViewBag.Empleados = EmpleadoCln.Listar("")
                .Where(e => e.estado == 1)
                .Select(e => new SelectListItem
                {
                    Value = e.id.ToString(),
                    Text = $"{e.nombres} {e.primerApellido} {e.segundoApellido}".Trim()
                }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Usuario usuario)
        {
            if (UsuarioCln.ExisteEmpleadoConUsuario(usuario.idEmpleado))
            {
                ModelState.AddModelError("", "Este empleado ya tiene un usuario asignado.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = EmpleadoCln.Listar("")
                    .Where(e => e.estado == 1)
                    .Select(e => new SelectListItem
                    {
                        Value = e.id.ToString(),
                        Text = $"{e.nombres} {e.primerApellido} {e.segundoApellido}".Trim()
                    }).ToList();

                return View(usuario);
            }

            usuario.fechaRegistro = DateTime.Now;
            usuario.estado = 1;
            UsuarioCln.Insertar(usuario);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Editar(int id)
        {
            var usuario = UsuarioCln.Obtener(id);
            if (usuario == null)
                return NotFound();

            ViewBag.Empleados = EmpleadoCln.Listar("")
                .Where(e => e.estado == 1)
                .Select(e => new SelectListItem
                {
                    Value = e.id.ToString(),
                    Text = $"{e.nombres} {e.primerApellido} {e.segundoApellido}".Trim(),
                    Selected = e.id == usuario.idEmpleado
                }).ToList();

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = EmpleadoCln.Listar("")
                    .Where(e => e.estado == 1)
                    .Select(e => new SelectListItem
                    {
                        Value = e.id.ToString(),
                        Text = $"{e.nombres} {e.primerApellido} {e.segundoApellido}".Trim(),
                        Selected = e.id == usuario.idEmpleado
                    }).ToList();

                return View(usuario);
            }

            UsuarioCln.Modificar(usuario);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Eliminar(int id)
        {
            UsuarioCln.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ValidarEmpleado(int idEmpleado)
        {
            var existe = UsuarioCln.ExisteEmpleadoConUsuario(idEmpleado);
            return Json(new { existe });
        }

    }
}
