using System;
using System.Collections.Generic;
using System.Linq;

namespace CadPizzeria
{
    public class ClienteCad
    {
        public static List<Cliente> Listar(string filtro)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Cliente
                    .Where(c => c.estado != -1 &&
                        (c.cedulaIdentidad.Contains(filtro) ||
                         c.nombres.Contains(filtro) ||
                         c.primerApellido.Contains(filtro) ||
                         c.segundoApellido.Contains(filtro)))
                    .OrderByDescending(c => c.estado)
                    .ThenBy(c => c.nombres)
                    .ToList();
            }
        }

        public static void Insertar(Cliente cliente)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                cliente.estado = 1;
                cliente.fechaRegistro = DateTime.Now;
                ctx.Cliente.Add(cliente);
                ctx.SaveChanges();
            }
        }

        public static Cliente Obtener(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Cliente.Find(id);
            }
        }

        public static void Actualizar(Cliente cliente)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var original = ctx.Cliente.Find(cliente.id);
                if (original != null)
                {
                    original.cedulaIdentidad = cliente.cedulaIdentidad;
                    original.nombres = cliente.nombres;
                    original.primerApellido = cliente.primerApellido;
                    original.segundoApellido = cliente.segundoApellido;
                    original.celular = cliente.celular;
                    original.direccion = cliente.direccion;
                    // NO actualizamos fecha ni estado
                    ctx.SaveChanges();
                }
            }
        }

        public static void Eliminar(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var cliente = ctx.Cliente.Find(id);
                if (cliente != null)
                {
                    cliente.estado = -1;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
