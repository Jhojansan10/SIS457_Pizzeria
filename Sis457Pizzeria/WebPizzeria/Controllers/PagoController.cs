using Microsoft.AspNetCore.Mvc;
using ClnPizzeria;
using CadPizzeria;
using WebPizzeria.Filters;

namespace WebPizzeria.Controllers
{
    public class PagoController : Controller
    {
        [AuthorizeByRole("Administrador")]
        public IActionResult Index(string filtro)
        {
            var lista = PagoCln.Listar(filtro ?? "");
            return View(lista);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Pago pago)
        {
            if (ModelState.IsValid)
            {
                PagoCln.Insertar(pago);
                return RedirectToAction(nameof(Index));
            }
            return View(pago);
        }

        public IActionResult Editar(int id)
        {
            var pago = PagoCln.Obtener(id);
            return View(pago);
        }

        [HttpPost]
        public IActionResult Editar(Pago pago)
        {
            PagoCln.Actualizar(pago);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Eliminar(int id)
        {
            PagoCln.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
