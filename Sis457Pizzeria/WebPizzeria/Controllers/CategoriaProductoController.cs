using Microsoft.AspNetCore.Mvc;
using ClnPizzeria;
using CadPizzeria;
using Microsoft.AspNetCore.Mvc.Filters;
using WebPizzeria.Filters;

namespace WebPizzeria.Controllers
{
    [AuthorizeByRole("Administrador")]
    public class CategoriaProductoController : Controller
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
            var lista = CategoriaProductoCln.Listar(filtro ?? "");
            return View(lista);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(CategoriaProducto categoria)
        {
            if (ModelState.IsValid)
            {
                CategoriaProductoCln.Insertar(categoria);
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        public IActionResult Editar(int id)
        {
            var categoria = CategoriaProductoCln.Obtener(id);
            return View(categoria);
        }

        [HttpPost]
        public IActionResult Editar(CategoriaProducto categoria)
        {
            CategoriaProductoCln.Actualizar(categoria);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Eliminar(int id)
        {
            CategoriaProductoCln.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
