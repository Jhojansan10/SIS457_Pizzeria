using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CadPizzeria;
using ClnPizzeria;

namespace WebPizzeria.Controllers
{
    public class PedidoController : Controller
    {
        public IActionResult Index()
        {
            var lista = PedidoCln.Listar();
            return View(lista);
        }

        public IActionResult Crear()
        {
            var productos = ProductoCln.Listar("")
                .Where(p => p.estado == 1 && p.stock > 0)
                .Select(p => new {
                    p.id,
                    nombre = p.descripcion,
                    p.precioVenta,
                    p.stock
                }).ToList();

            ViewBag.Productos = productos;
            return View();
        }

        public IActionResult Entregar(int id)
        {
            try
            {
                var pedido = PedidoCln.Obtener(id);
                if (pedido != null && pedido.estadoEntrega != "Entregado")
                {
                    pedido.estadoEntrega = "Entregado";
                    PedidoCln.Actualizar(pedido);
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return StatusCode(500, "Error al marcar como entregado.");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(string cedulaIdentidad, string nombreCompleto, string metodoPago, decimal montoRecibido, List<DetallePedido> detalles)
        {
            if (detalles == null || detalles.Count == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un producto.");
                return View();
            }

            // Verificar si el cliente existe por CI
            var clientes = ClienteCln.Listar(cedulaIdentidad);
            Cliente cliente;

            if (clientes.Count > 0)
            {
                cliente = clientes.First();
            }
            else
            {
                // Separar el nombre completo
                var partes = nombreCompleto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string nombres = "", primerApellido = "", segundoApellido = "";

                if (partes.Length == 1)
                {
                    nombres = partes[0];
                }
                else if (partes.Length == 2)
                {
                    nombres = partes[0];
                    primerApellido = partes[1];
                }
                else if (partes.Length == 3)
                {
                    nombres = partes[0];
                    primerApellido = partes[1];
                    segundoApellido = partes[2];
                }
                else if (partes.Length >= 4)
                {
                    nombres = partes[0] + " " + partes[1];
                    primerApellido = partes[2];
                    segundoApellido = partes[3];
                }

                cliente = new Cliente
                {
                    cedulaIdentidad = cedulaIdentidad,
                    nombres = nombres,
                    primerApellido = primerApellido,
                    segundoApellido = segundoApellido,
                    fechaRegistro = DateTime.Now,
                    estado = 1
                };
                ClienteCln.Insertar(cliente);
                cliente.id = ClienteCln.Listar(cedulaIdentidad).First().id;
            }

            var pedido = new Pedido
            {
                idCliente = cliente.id,
                modoEntrega = "Local",
                estadoEntrega = "Pendiente",
                total = 0,
                fechaPedido = DateTime.Now,
                estado = 1
            };

            try
            {
                PedidoCln.Insertar(pedido, detalles);

                var totalPedido = pedido.total;
                if (metodoPago == "Efectivo" && montoRecibido < totalPedido)
                {
                    ModelState.AddModelError("", "El monto ingresado es menor al total del pedido.");
                    return View();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al registrar pedido: {ex.Message}");
                return View();
            }
        }

        // NUEVO: Método para mostrar el recibo como vista parcial
        public IActionResult Recibo(int id)
        {
            var pedido = PedidoCln.Obtener(id);
            if (pedido == null)
                return NotFound();

            return PartialView("_Recibo", pedido); // Asegúrate de que la vista se llame _Recibo.cshtml
        }
    }
}
