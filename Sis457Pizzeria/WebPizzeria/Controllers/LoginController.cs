using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CadPizzeria;
using ClnPizzeria;
using System.Linq;

namespace WebPizzeria.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Empleado()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Empleado(string usuarioLogin, string clave)
        {
            var usuario = UsuarioCln.Listar("")
                .FirstOrDefault(u => u.usuarioLogin == usuarioLogin && u.clave == clave && u.estado != -1);

            if (usuario != null)
            {
                // Obtener el empleado relacionado
                var empleado = EmpleadoCln.Obtener(usuario.idEmpleado);

                // Guardar en sesión
                HttpContext.Session.SetInt32("UsuarioId", usuario.id);
                HttpContext.Session.SetString("UsuarioNombre", empleado.nombres);
                HttpContext.Session.SetString("UsuarioRol", empleado.cargo); 

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }


        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Empleado", "Login");
        }

        public IActionResult Cliente()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cliente(string usuarioLogin, string clave)
        {
            var usuario = UsuarioClienteCln.Listar("")
                .FirstOrDefault(u => u.usuarioLogin == usuarioLogin && u.clave == clave && u.estado != -1);

            if (usuario != null)
            {
                HttpContext.Session.SetInt32("ClienteId", usuario.idCliente);
                HttpContext.Session.SetString("ClienteNombre", usuario.Cliente.nombres);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        public IActionResult CerrarSesionCliente()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Cliente");
        }


        [HttpGet]
        public IActionResult RegistroCliente()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegistroCliente(string ci, string nombres, string primerApellido, string segundoApellido, string usuarioLogin, string clave)
        {
            // Verificar si ya existe el CI
            var yaExisteCI = ClienteCln.Listar(ci).Any(c => c.estado != -1);
            var yaExisteUsuario = UsuarioClienteCln.Listar("").Any(u => u.usuarioLogin == usuarioLogin && u.estado != -1);

            if (yaExisteCI)
            {
                ViewBag.Error = "Ya existe un cliente registrado con ese CI.";
                return View();
            }

            if (yaExisteUsuario)
            {
                ViewBag.Error = "Ese nombre de usuario ya está en uso.";
                return View();
            }

            // Crear nuevo cliente
            var cliente = new Cliente
            {
                cedulaIdentidad = ci,
                nombres = nombres,
                primerApellido = primerApellido,
                segundoApellido = segundoApellido,
                fechaRegistro = DateTime.Now,
                estado = 1
            };
            ClienteCln.Insertar(cliente);
            var nuevo = ClienteCln.Listar(ci).First();

            // Crear nuevo usuario cliente
            var usuario = new UsuarioCliente
            {
                idCliente = nuevo.id,
                usuarioLogin = usuarioLogin,
                clave = clave,
                fechaRegistro = DateTime.Now,
                estado = 1
            };
            UsuarioClienteCln.Insertar(usuario);

            // Guardar sesión
            HttpContext.Session.SetInt32("ClienteId", nuevo.id);
            HttpContext.Session.SetString("ClienteNombre", nuevo.nombres);

            return RedirectToAction("Index", "Home");
        }




    }
}
