using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace CadPizzeria
{
    public class DireccionCad
    {
        public static List<Direccion> Listar()
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Direccion
                    .Include(d => d.Cliente)
                    .Where(d => d.estado != -1)
                    .OrderByDescending(d => d.fechaRegistro)
                    .ToList();
            }
        }

        public static void Insertar(Direccion direccion)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                direccion.fechaRegistro = System.DateTime.Now;
                direccion.estado = 1;
                ctx.Direccion.Add(direccion);

                var cliente = ctx.Cliente.Find(direccion.idCliente);
                if (cliente != null)
                {
                    cliente.direccion = direccion.calle;
                }

                ctx.SaveChanges();
            }
        }

        public static Direccion Obtener(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Direccion.Find(id);
            }
        }

        public static void Actualizar(Direccion direccion)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var original = ctx.Direccion.Find(direccion.id);
                if (original != null)
                {
                    direccion.fechaRegistro = original.fechaRegistro;
                    direccion.estado = original.estado;
                    ctx.Entry(original).CurrentValues.SetValues(direccion);

                    var cliente = ctx.Cliente.Find(direccion.idCliente);
                    if (cliente != null)
                    {
                        cliente.direccion = direccion.calle;
                    }

                    ctx.SaveChanges();
                }
            }
        }

        public static void Eliminar(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var direccion = ctx.Direccion.Find(id);
                if (direccion != null)
                {
                    direccion.estado = -1;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
