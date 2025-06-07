using CadPizzeria;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClnPizzeria
{
    public static class DireccionCln
    {
        /// <summary>
        /// Trae todas las direcciones activas.
        /// </summary>
        public static List<object> Listar()
        {
            using (var db = new LabPizzeriaEntities())
            {
                return db.DIRECCION
                         .Where(d => d.estado)
                         .OrderBy(d => d.USUARIO.nombre)
                         .Select(d => new
                         {
                             d.direccion_id,
                             Cliente = d.USUARIO.nombre,
                             d.calle,
                             d.ciudad,
                             d.codigo_postal,
                             d.indicaciones
                         })
                         .ToList<object>();
            }
        }

        /// <summary>
        /// Busca direcciones por nombre de cliente o ciudad.
        /// </summary>
        public static List<object> Buscar(string criterio)
        {
            criterio = criterio.ToLower().Trim();
            using (var db = new LabPizzeriaEntities())
            {
                return db.DIRECCION
                         .Where(d => d.estado &&
                                     (d.USUARIO.nombre.ToLower().Contains(criterio) ||
                                      d.ciudad.ToLower().Contains(criterio)))
                         .OrderBy(d => d.USUARIO.nombre)
                         .Select(d => new
                         {
                             d.direccion_id,
                             Cliente = d.USUARIO.nombre,
                             d.calle,
                             d.ciudad,
                             d.codigo_postal,
                             d.indicaciones
                         })
                         .ToList<object>();
            }
        }

        /// <summary>
        /// Inserta una nueva dirección y retorna su ID.
        /// </summary>
        public static int Insertar(DIRECCION direccion)
        {
            using (var db = new LabPizzeriaEntities())
            {
                db.DIRECCION.Add(direccion);
                db.SaveChanges();
                return direccion.direccion_id;
            }
        }

        /// <summary>
        /// Actualiza los datos de una dirección existente.
        /// </summary>
        public static void Actualizar(DIRECCION direccion)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var orig = db.DIRECCION.Find(direccion.direccion_id);
                if (orig == null) return;
                orig.usuario_id = direccion.usuario_id;
                orig.calle = direccion.calle;
                orig.ciudad = direccion.ciudad;
                orig.codigo_postal = direccion.codigo_postal;
                orig.indicaciones = direccion.indicaciones;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Marca una dirección como inactiva (eliminación lógica).
        /// </summary>
        public static void Eliminar(int idDireccion)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var d = db.DIRECCION.Find(idDireccion);
                if (d == null) return;
                d.estado = false;
                db.SaveChanges();
            }
        }
    }
}
