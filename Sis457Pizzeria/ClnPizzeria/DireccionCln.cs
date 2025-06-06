using CadPizzeria;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClnPizzeria
{
    public class DireccionCln
    {
        public static List<object> listar()
        {
            using (var db = new LabPizzeriaEntities())
            {
                return db.DIRECCION
                    .Where(d => d.estado == true)
                    .OrderBy(d => d.USUARIO.nombre)
                    .Select(d => new
                    {
                        d.direccion_id,
                        Cliente = d.USUARIO.nombre,
                        d.calle,
                        d.ciudad,
                        d.codigo_postal,
                        d.indicaciones
                    }).ToList<object>();
            }
        }

        public static List<object> buscar(string criterio)
        {
            using (var db = new LabPizzeriaEntities())
            {
                return db.DIRECCION
                    .Where(d => d.estado == true &&
                        (d.USUARIO.nombre.ToLower().Contains(criterio.ToLower()) ||
                         d.ciudad.ToLower().Contains(criterio.ToLower())))
                    .OrderBy(d => d.USUARIO.nombre)
                    .Select(d => new
                    {
                        d.direccion_id,
                        Cliente = d.USUARIO.nombre,
                        d.calle,
                        d.ciudad,
                        d.codigo_postal,
                        d.indicaciones
                    }).ToList<object>();
            }
        }

        public static int insertar(DIRECCION direccion)
        {
            using (var db = new LabPizzeriaEntities())
            {
                direccion.estado = true;
                db.DIRECCION.Add(direccion);
                db.SaveChanges();
                return direccion.direccion_id;
            }
        }

        public static void actualizar(DIRECCION direccion)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var original = db.DIRECCION.Find(direccion.direccion_id);
                original.usuario_id = direccion.usuario_id;
                original.calle = direccion.calle;
                original.ciudad = direccion.ciudad;
                original.codigo_postal = direccion.codigo_postal;
                original.indicaciones = direccion.indicaciones;
                db.SaveChanges();
            }
        }

        public static void eliminar(int id)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var direccion = db.DIRECCION.Find(id);
                direccion.estado = false;
                db.SaveChanges();
            }
        }
    }
}
