using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace CadPizzeria
{
    public class UsuarioCad
    {
        public static List<Usuario> Listar(string filtro)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Usuario
                    .Include(u => u.Empleado)
                    .Where(u => u.estado != -1 &&
                        (u.usuarioLogin.Contains(filtro) ||
                         u.Empleado.nombres.Contains(filtro) ||
                         u.Empleado.primerApellido.Contains(filtro)))
                    .OrderByDescending(u => u.fechaRegistro)
                    .ToList();
            }
        }

        public static Usuario Obtener(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Usuario
                    .Include(u => u.Empleado) // 👈 Esto es clave
                    .FirstOrDefault(u => u.id == id && u.estado != -1);
            }
        }

        public static void Insertar(Usuario usuario)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                usuario.fechaRegistro = DateTime.Now;
                usuario.estado = 1;
                ctx.Usuario.Add(usuario);
                ctx.SaveChanges();
            }
        }

        public static void Modificar(Usuario usuario)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var actual = ctx.Usuario.Find(usuario.id);
                if (actual != null)
                {
                    actual.usuarioLogin = usuario.usuarioLogin;
                    actual.clave = usuario.clave;
                    actual.idEmpleado = usuario.idEmpleado;
                    ctx.SaveChanges();
                }
            }
        }

        public static void Eliminar(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var actual = ctx.Usuario.Find(id);
                if (actual != null)
                {
                    actual.estado = -1;
                    ctx.SaveChanges();
                }
            }
        }

        public static bool ExisteEmpleadoConUsuario(int idEmpleado)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Usuario.Any(u => u.idEmpleado == idEmpleado && u.estado != -1);
            }
        }

    }
}
