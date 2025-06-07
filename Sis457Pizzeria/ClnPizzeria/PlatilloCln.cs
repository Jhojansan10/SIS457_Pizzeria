using CadPizzeria;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClnPizzeria
{
    public class PlatilloCln
    {
        public static List<PlatilloDto> listar()
        {
            using (var db = new LabPizzeriaEntities())
            {
                return db.PLATILLO
                    .Where(p => p.estado == true)
                    .OrderBy(p => p.nombre)
                    .Select(p => new PlatilloDto
                    {
                        platillo_id = p.platillo_id,
                        nombre = p.nombre,
                        descripcion = p.descripcion,
                        precio = p.precio,
                        tiempo_preparacion = p.tiempo_preparacion,
                        imagen_url = p.imagen_url,
                        categoria = p.CATEGORIA.nombre,
                        estado = p.estado
                    }).ToList();
            }
        }


        public static int insertar(PLATILLO platillo)
        {
            using (var db = new LabPizzeriaEntities())
            {
                platillo.estado = true;
                db.PLATILLO.Add(platillo);
                db.SaveChanges();
                return platillo.platillo_id;
            }
        }

        public static void actualizar(PLATILLO platillo)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var original = db.PLATILLO.Find(platillo.platillo_id);
                if (original == null)
                {
                    throw new Exception("El platillo que intenta actualizar no existe.");
                }

                // Solo si original no es null, se actualiza
                original.nombre = platillo.nombre;
                original.descripcion = platillo.descripcion;
                original.precio = platillo.precio;
                original.tiempo_preparacion = platillo.tiempo_preparacion;
                original.imagen_url = platillo.imagen_url;
                original.categoria_id = platillo.categoria_id;
                original.estado = platillo.estado;
                db.SaveChanges();
            }
        }

        public static void eliminar(int id)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var platillo = db.PLATILLO.Find(id);
                platillo.estado = false;
                db.SaveChanges();
            }
        }
    }
}
