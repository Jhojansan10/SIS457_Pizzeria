using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using CadPizzeria;
using ClnPizzeria;
using WebPizzeria.Filters;

namespace WebPizzeria.Controllers
{
    [AuthorizeByRole("Administrador", "Empleado", "Repartidor")]
    public class PedidoController : Controller
    {
        [AuthorizeByRole("Administrador", "Empleado", "Repartidor")]
        public IActionResult Index()
        {
            var lista = PedidoCln.Listar();
            return View(lista);
        }

        [AuthorizeByRole("Administrador", "Empleado")]
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

            var repartidores = EmpleadoCln.Listar("")
                .Where(e => e.estado == 1 && e.cargo == "Repartidor")
                .Select(e => new {
                    e.id,
                    nombre = $"{e.nombres} {e.primerApellido} {e.segundoApellido}"
                }).ToList();

            ViewBag.Productos = productos;
            ViewBag.Repartidores = repartidores;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeByRole("Administrador", "Empleado")]
        public IActionResult Crear(string cedulaIdentidad, string nombreCompleto, string metodoPago, decimal montoRecibido, string modoEntrega, int? idRepartidor, string direccionTexto, List<DetallePedido> detalles)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
            {
                ModelState.AddModelError("", "Debe ingresar el nombre del cliente.");
                return View();
            }

            if (detalles == null || detalles.Count == 0)
            {
                ModelState.AddModelError("", "Debe seleccionar al menos un producto.");
                return View();
            }

            Cliente cliente;
            var clientes = !string.IsNullOrWhiteSpace(cedulaIdentidad)
                ? ClienteCln.Listar(cedulaIdentidad)
                : new List<Cliente>();

            if (clientes.Count > 0)
            {
                cliente = clientes.First();
            }
            else
            {
                if (string.IsNullOrWhiteSpace(cedulaIdentidad))
                {
                    var listaSci = ClienteCln.Listar("")
                        .Where(c => c.cedulaIdentidad.StartsWith("SCI"))
                        .OrderByDescending(c => c.cedulaIdentidad)
                        .ToList();

                    int numero = 1;
                    if (listaSci.Any())
                    {
                        var ultimo = listaSci.First().cedulaIdentidad;
                        if (int.TryParse(ultimo.Replace("SCI", ""), out int n))
                            numero = n + 1;
                    }
                    cedulaIdentidad = $"SCI{numero}";
                }

                var partes = nombreCompleto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                string nombres = "", primerApellido = "", segundoApellido = "";

                if (partes.Length == 1)
                    nombres = partes[0];
                else if (partes.Length == 2)
                {
                    nombres = partes[0]; primerApellido = partes[1];
                }
                else if (partes.Length == 3)
                {
                    nombres = partes[0]; primerApellido = partes[1]; segundoApellido = partes[2];
                }
                else if (partes.Length >= 4)
                {
                    nombres = partes[0] + " " + partes[1];
                    primerApellido = partes[2]; segundoApellido = partes[3];
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

            int? idDireccion = null;
            if (modoEntrega == "Delivery")
            {
                var direcciones = DireccionCln.Listar()
                    .Where(d => d.estado == 1 && d.idCliente == cliente.id && d.calle.Equals(direccionTexto, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                if (direcciones.Any())
                {
                    idDireccion = direcciones.First().id;
                }
                else
                {
                    var nuevaDireccion = new Direccion
                    {
                        idCliente = cliente.id,
                        calle = direccionTexto,
                        indicaciones = "",
                        fechaRegistro = DateTime.Now,
                        estado = 1
                    };
                    DireccionCln.Insertar(nuevaDireccion);

                    idDireccion = DireccionCln.Listar()
                        .Where(d => d.idCliente == cliente.id && d.calle == direccionTexto)
                        .OrderByDescending(d => d.id)
                        .First().id;
                }
            }

            var pedido = new Pedido
            {
                idCliente = cliente.id,
                modoEntrega = modoEntrega,
                idRepartidor = idRepartidor,
                idDireccion = idDireccion,
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

        [HttpGet]
        [AuthorizeByRole("Administrador", "Empleado")]
        public IActionResult ObtenerCIs(string term)
        {
            var lista = ClienteCln.Listar("")
                .Where(c => c.cedulaIdentidad.StartsWith(term))
                .Select(c => new { value = c.cedulaIdentidad, nombre = $"{c.nombres} {c.primerApellido}" })
                .ToList();

            return Json(lista);
        }




        [AuthorizeByRole("Administrador", "Empleado", "Repartidor")]
        public IActionResult Recibo(int id)
        {
            try
            {
                var pedido = PedidoCln.Obtener(id);
                if (pedido == null)
                    return NotFound();

                return PartialView("_Recibo", pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [AuthorizeByRole("Administrador", "Empleado","Repartidor")]
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



    }
}


