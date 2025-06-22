using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace CadPizzeria
{
    public class UsuarioClienteCad
    {
        public static List<UsuarioCliente> Listar(string filtro)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.UsuarioCliente
                    .Include("Cliente") // ✅ carga explícita
                    .Where(u => u.estado != -1 &&
                        (u.usuarioLogin.Contains(filtro) ||
                         u.Cliente.nombres.Contains(filtro) ||
                         u.Cliente.primerApellido.Contains(filtro)))
                    .OrderByDescending(u => u.fechaRegistro)
                    .ToList();
            }
        }

        public static UsuarioCliente Obtener(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.UsuarioCliente.FirstOrDefault(u => u.id == id && u.estado != -1);
            }
        }

        public static void Insertar(UsuarioCliente usuario)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                usuario.fechaRegistro = DateTime.Now;
                usuario.estado = 1;
                ctx.UsuarioCliente.Add(usuario);
                ctx.SaveChanges();
            }
        }

        public static void Modificar(UsuarioCliente usuario)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var actual = ctx.UsuarioCliente.Find(usuario.id);
                if (actual != null)
                {
                    actual.usuarioLogin = usuario.usuarioLogin;
                    actual.clave = usuario.clave;
                    actual.idCliente = usuario.idCliente;
                    ctx.SaveChanges();
                }
            }
        }

        public static void Eliminar(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var actual = ctx.UsuarioCliente.Find(id);
                if (actual != null)
                {
                    actual.estado = -1;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
