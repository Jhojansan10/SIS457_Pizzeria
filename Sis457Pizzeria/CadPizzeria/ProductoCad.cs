using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace CadPizzeria
{
    public class ProductoCad
    {
        // Lista todo
        public static List<Producto> Listar()
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Producto
                    .Include(p => p.CategoriaProducto)
                    .Where(p => p.estado != -1)
                    .OrderByDescending(p => p.estado)
                    .ThenBy(p => p.descripcion)
                    .ToList();
            }
        }

        // Lista filtrando
        public static List<Producto> Listar(string filtro)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Producto
                    .Include(p => p.CategoriaProducto)
                    .Where(p => p.estado != -1 &&
                                (p.codigo.Contains(filtro) || p.descripcion.Contains(filtro) || p.CategoriaProducto.nombre.Contains(filtro)))
                    .OrderByDescending(p => p.estado)
                    .ThenBy(p => p.descripcion)
                    .ToList();
            }
        }

        public static void Insertar(Producto producto)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                producto.fechaRegistro = DateTime.Now;
                producto.estado = 1;
                ctx.Producto.Add(producto);
                ctx.SaveChanges();
            }
        }
        public static Producto Obtener(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Producto.Find(id);
            }
        }

        public static void Actualizar(Producto producto)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                ctx.Producto.Attach(producto);
                ctx.Entry(producto).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public static void Eliminar(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var producto = ctx.Producto.Find(id);
                if (producto != null)
                {
                    producto.estado = -1;
                    ctx.SaveChanges();
                }
            }
        }

    }
}
