using Microsoft.AspNetCore.Mvc;
using ClnPizzeria;
using CadPizzeria;

namespace WebPizzeria.Controllers
{
    public class ClienteController : Controller
    {
        public IActionResult Index(string filtro)
        {
            var lista = ClienteCln.Listar(filtro ?? "");
            return View(lista);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.estado = 1;
                cliente.fechaRegistro = DateTime.Now;
                ClienteCln.Insertar(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        public IActionResult Editar(int id)
        {
            var cliente = ClienteCln.Obtener(id);
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(Cliente cliente)
        {
            ClienteCln.Actualizar(cliente);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Eliminar(int id)
        {
            ClienteCln.Eliminar(id);
            return RedirectToAction(nameof(Index));
        }

     
        [HttpGet]
        public JsonResult AutocompletarCI(string term)
        {
            var clientes = ClienteCln.Listar("")
                .Where(c => c.cedulaIdentidad.StartsWith(term))
                .Select(c => new {
                    label = c.cedulaIdentidad,
                    value = c.cedulaIdentidad,
                    nombre = $"{c.nombres} {c.primerApellido} {c.segundoApellido}"
                }).ToList();

            return Json(clientes);
        }

    }
}
