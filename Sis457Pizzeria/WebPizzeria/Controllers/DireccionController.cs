using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClnPizzeria;
using CadPizzeria;
using Microsoft.AspNetCore.Mvc.Filters;
using WebPizzeria.Filters;

namespace WebPizzeria.Controllers
{
    [AuthorizeByRole("Administrador")]
    public class DireccionController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioNombre")))
            {
                context.Result = RedirectToAction("Empleado", "Login");
            }
            base.OnActionExecuting(context);
        }

        public IActionResult Index(string filtro)
        {
            var lista = DireccionCln.Listar()
                .Where(d => string.IsNullOrEmpty(filtro) ||
                            d.calle.ToLower().Contains(filtro.ToLower()) ||
                            d.indicaciones.ToLower().Contains(filtro.ToLower()))
                .ToList();

            return View(lista);
        }

        public IActionResult Crear()
        {
            var clientes = ClienteCln.Listar("")
                .Select(c => new {
                    c.id,
                    nombre = $"{c.nombres} {c.primerApellido} {c.segundoApellido}"
                })
                .ToList();

            ViewBag.Clientes = new SelectList(clientes, "id", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                direccion.estado = 1;
                direccion.fechaRegistro = DateTime.Now;
                DireccionCln.Insertar(direccion);
                return RedirectToAction(nameof(Index));
            }

            var clientes = ClienteCln.Listar("")
                .Select(c => new {
                    c.id,
                    nombre = $"{c.nombres} {c.primerApellido} {c.segundoApellido}"
                })
                .ToList();

            ViewBag.Clientes = new SelectList(clientes, "id", "nombre", direccion.idCliente);
            return View(direccion);
        }

        public IActionResult Editar(int id)
        {
            var direccion = DireccionCln.Obtener(id);

            var clientes = ClienteCln.Listar("")
                .Select(c => new {
                    c.id,
                    nombre = $"{c.nombres} {c.primerApellido} {c.segundoApellido}"
                })
                .ToList();

            ViewBag.Clientes = new SelectList(clientes, "id", "nombre", direccion.idCliente);
            return View(direccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Direccion direccion)
        {
            if (ModelState.IsValid)
            {
                DireccionCln.Actualizar(direccion);
                return RedirectToAction(nameof(Index));
            }

            var clientes = ClienteCln.Listar("")
                .Select(c => new {
                    c.id,
                    nombre = $"{c.nombres} {c.primerApellido} {c.segundoApellido}"
                })
                .ToList();

            ViewBag.Clientes = new SelectList(clientes, "id", "nombre", direccion.idCliente);
            return View(direccion);
        }

        public IActionResult Eliminar(int id)
        {
            DireccionCln.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
