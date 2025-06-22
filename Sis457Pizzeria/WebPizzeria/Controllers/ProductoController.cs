using Microsoft.AspNetCore.Mvc;
using ClnPizzeria;
using CadPizzeria;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Filters;
using WebPizzeria.Filters;

namespace WebPizzeria.Controllers
{
    [AuthorizeByRole("Administrador")]
    public class ProductoController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioNombre")))
            {
                context.Result = RedirectToAction("Empleado", "Login");
            }
            base.OnActionExecuting(context);
        }

        private void CargarCategorias()
        {
            var categorias = CategoriaProductoCln.Listar("");
            ViewBag.Categorias = new SelectList(categorias, "id", "nombre");
        }

        // GET: Producto/Crear
        public IActionResult Crear()
        {
            CargarCategorias();
            return View();
        }

        // POST: Producto/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.estado = 1; // el estado se asigna automáticamente
                ProductoCln.Insertar(producto);
                return RedirectToAction("Index");
            }

            CargarCategorias();
            return View(producto);
        }

        public IActionResult Editar(int id)
        {
            var producto = ProductoCln.Obtener(id);
            CargarCategorias();
            return View(producto);
        }

        [HttpPost]
        public IActionResult Editar(Producto producto)
        {
            ProductoCln.Actualizar(producto);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Eliminar(int id)
        {
            ProductoCln.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index(string filtro)
        {
            var lista = ProductoCln.Listar(filtro ?? "");
            return View(lista);
        }

    }
}
