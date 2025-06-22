using Microsoft.AspNetCore.Mvc;
using CadPizzeria;
using ClnPizzeria;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebPizzeria.Controllers
{
    public class UsuarioClienteController : Controller
    {
        public IActionResult Index(string filtro)
        {
            var lista = UsuarioClienteCln.Listar(filtro ?? "");
            ViewBag.Filtro = filtro;
            return View(lista);
        }

        public IActionResult Crear()
        {
            ViewBag.Clientes = ObtenerClientesSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(UsuarioCliente usuarioCliente)
        {
            // Verificar si ya existe un usuario para ese cliente
            var existe = UsuarioClienteCln.Listar("")
                .Any(u => u.idCliente == usuarioCliente.idCliente && u.estado != -1);

            if (existe)
            {
                ViewBag.Clientes = ObtenerClientesSelectList();
                ModelState.AddModelError("", "Este cliente ya tiene un usuario asignado.");
                return View(usuarioCliente);
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = ObtenerClientesSelectList();
                return View(usuarioCliente);
            }

            usuarioCliente.fechaRegistro = DateTime.Now;
            usuarioCliente.estado = 1;
            UsuarioClienteCln.Insertar(usuarioCliente);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Editar(int id)
        {
            var usuario = UsuarioClienteCln.Obtener(id);
            if (usuario == null)
                return NotFound();

            ViewBag.Clientes = ObtenerClientesSelectList();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(UsuarioCliente usuarioCliente)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Clientes = ObtenerClientesSelectList();
                return View(usuarioCliente);
            }

            UsuarioClienteCln.Modificar(usuarioCliente);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Eliminar(int id)
        {
            UsuarioClienteCln.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

        // ✅ Nuevo método para validación AJAX
        [HttpGet]
        public JsonResult VerificarCliente(int idCliente)
        {
            var existe = UsuarioClienteCln.Listar("")
                .Any(u => u.idCliente == idCliente && u.estado != -1);
            return Json(existe);
        }

        // 🔁 Método privado para evitar duplicar código
        private List<SelectListItem> ObtenerClientesSelectList()
        {
            return ClienteCln.Listar("")
                .Where(c => c.estado == 1)
                .Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = $"{c.nombres} {c.primerApellido} {c.segundoApellido}".Trim()
                })
                .ToList();
        }
    }
}
