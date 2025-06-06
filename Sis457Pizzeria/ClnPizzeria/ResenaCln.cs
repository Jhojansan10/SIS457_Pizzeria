using CadPizzeria;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClnPizzeria
{
    public class ResenaCln
    {
        public static List<object> listar()
        {
            using (var db = new LabPizzeriaEntities())
            {
                return db.RESENA
                    .Where(r => r.estado_registro == true)
                    .OrderByDescending(r => r.fecha)
                    .Select(r => new
                    {
                        r.resena_id,
                        Cliente = r.USUARIO.nombre,
                        Pedido = r.pedido_id,
                        Calificación = r.calificacion,
                        Comentario = r.comentario,
                        Fecha = r.fecha
                    })
                    .ToList<object>();
            }
        }

        public static List<object> buscar(string criterio)
        {
            using (var db = new LabPizzeriaEntities())
            {
                return db.RESENA
                    .Where(r => r.estado_registro == true &&
                        (r.USUARIO.nombre.ToLower().Contains(criterio.ToLower()) ||
                         r.comentario.ToLower().Contains(criterio.ToLower()) ||
                         r.calificacion.ToString().Contains(criterio)))
                    .OrderByDescending(r => r.fecha)
                    .Select(r => new
                    {
                        r.resena_id,
                        Cliente = r.USUARIO.nombre,
                        Pedido = r.pedido_id,
                        Calificación = r.calificacion,
                        Comentario = r.comentario,
                        Fecha = r.fecha
                    })
                    .ToList<object>();
            }
        }

        public static int insertar(RESENA resena)
        {
            using (var db = new LabPizzeriaEntities())
            {
                db.RESENA.Add(resena);
                db.SaveChanges();
                return resena.resena_id;
            }
        }

        public static void actualizar(RESENA resena)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var original = db.RESENA.Find(resena.resena_id);
                original.usuario_id = resena.usuario_id;
                original.pedido_id = resena.pedido_id;
                original.calificacion = resena.calificacion;
                original.comentario = resena.comentario;
                original.fecha = resena.fecha;
                db.SaveChanges();
            }
        }

        public static void eliminar(int id)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var resena = db.RESENA.Find(id);
                resena.estado_registro = false;
                db.SaveChanges();
            }
        }
    }
}
