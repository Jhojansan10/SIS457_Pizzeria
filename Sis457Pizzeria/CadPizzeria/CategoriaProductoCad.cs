using System;
using System.Collections.Generic;
using System.Linq;

namespace CadPizzeria
{
    public class CategoriaProductoCad
    {
        public static List<CategoriaProducto> Listar(string filtro)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.CategoriaProducto
                    .Where(c => c.estado != -1 &&
                        (c.nombre + " " + c.descripcion).ToLower().Contains(filtro.ToLower()))
                    .OrderByDescending(c => c.estado)
                    .ThenBy(c => c.nombre)
                    .ToList();
            }
        }

        public static CategoriaProducto Obtener(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.CategoriaProducto.Find(id);
            }
        }

        public static void Insertar(CategoriaProducto categoria)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                categoria.estado = 1;
                categoria.fechaRegistro = DateTime.Now;
                ctx.CategoriaProducto.Add(categoria);
                ctx.SaveChanges();
            }
        }

        public static void Actualizar(CategoriaProducto categoria)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                ctx.CategoriaProducto.Attach(categoria);
                ctx.Entry(categoria).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static void Eliminar(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var categoria = ctx.CategoriaProducto.Find(id);
                if (categoria != null)
                {
                    categoria.estado = -1;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
