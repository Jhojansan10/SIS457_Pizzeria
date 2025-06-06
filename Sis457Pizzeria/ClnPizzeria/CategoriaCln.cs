using CadPizzeria;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClnPizzeria
{
    public class CategoriaCln
    {
        public static List<CategoriaDto> listar()
        {
            using (var db = new LabPizzeriaEntities())
            {
                return db.CATEGORIA
                    .Where(c => c.estado == true)
                    .OrderBy(c => c.nombre)
                    .Select(c => new CategoriaDto
                    {
                        categoria_id = c.categoria_id,
                        nombre = c.nombre,
                        descripcion = c.descripcion,
                        estado = c.estado
                    })
                    .ToList();
            }
        }

        public static int insertar(CATEGORIA categoria)
        {
            using (var db = new LabPizzeriaEntities())
            {
                if (db.CATEGORIA.Any(c => c.nombre == categoria.nombre && c.estado == true))
                    throw new Exception("Ya existe una categoría con este nombre.");

                categoria.estado = true;
                db.CATEGORIA.Add(categoria);
                db.SaveChanges();
                return categoria.categoria_id;
            }
        }

        public static void actualizar(CATEGORIA categoria)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var original = db.CATEGORIA.Find(categoria.categoria_id);
                original.nombre = categoria.nombre;
                original.descripcion = categoria.descripcion;
                original.estado = categoria.estado;
                db.SaveChanges();
            }
        }

        public static void eliminar(int id)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var categoria = db.CATEGORIA.Find(id);
                categoria.estado = false;
                db.SaveChanges();
            }
        }
    }
}

