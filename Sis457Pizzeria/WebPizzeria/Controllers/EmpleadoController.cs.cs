using Microsoft.AspNetCore.Mvc;
using CadPizzeria;
using ClnPizzeria;

namespace WebPizzeria.Controllers
{
    public class EmpleadoController : Controller
    {
        public IActionResult Index(string filtro = "")
        {
            var lista = EmpleadoCln.Listar(filtro);
            ViewBag.Filtro = filtro;
            return View(lista);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                EmpleadoCln.Insertar(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public IActionResult Editar(int id)
        {
            var empleado = EmpleadoCln.Obtener(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                EmpleadoCln.Modificar(empleado);
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public IActionResult Eliminar(int id)
        {
            EmpleadoCln.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}
