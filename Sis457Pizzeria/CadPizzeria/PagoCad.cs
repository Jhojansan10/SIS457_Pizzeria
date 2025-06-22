using System.Collections.Generic;
using System.Linq;

namespace CadPizzeria
{
    public class PagoCad
    {
        public static List<Pago> Listar(string filtro)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Pago
                    .Where(p => p.estado != -1 &&
                        (p.metodoPago.Contains(filtro)))
                    .OrderByDescending(p => p.estado)
                    .ThenBy(p => p.metodoPago)
                    .ToList();
            }
        }

        public static void Insertar(Pago pago)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                pago.estado = 1;
                ctx.Pago.Add(pago);
                ctx.SaveChanges();
            }
        }

        public static Pago Obtener(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Pago.Find(id);
            }
        }

        public static void Actualizar(Pago pago)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                ctx.Entry(pago).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static void Eliminar(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var pago = ctx.Pago.Find(id);
                if (pago != null)
                {
                    pago.estado = -1;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
