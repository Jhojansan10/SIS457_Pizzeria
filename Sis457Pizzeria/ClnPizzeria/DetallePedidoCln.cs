using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CadPizzeria;   // Namespace donde están LabPizzeriaEntities y DETALLE_PEDIDO

namespace ClnPizzeria
{
    public class DetallePedidoCln
    {
        /// <summary>
        /// Inserta un nuevo DETALLE_PEDIDO y devuelve su ID (detalle_id).
        /// </summary>
        public static int Insertar(DETALLE_PEDIDO detalle)
        {
            using (var context = new LabPizzeriaEntities())
            {
                context.DETALLE_PEDIDO.Add(detalle);
                context.SaveChanges();
                return detalle.detalle_id;
            }
        }

        /// <summary>
        /// Actualiza un DETALLE_PEDIDO existente.
        /// </summary>
        public static int Actualizar(DETALLE_PEDIDO detalle)
        {
            using (var context = new LabPizzeriaEntities())
            {
                // Busca por clave primaria (detalle_id)
                var existente = context.DETALLE_PEDIDO.Find(detalle.detalle_id);
                if (existente == null) return 0;

                // Asigna los valores a las propiedades reales (tal como aparecen en DETALLE_PEDIDO.cs)
                existente.pedido_id = detalle.pedido_id;
                existente.platillo_id = detalle.platillo_id;
                existente.cantidad = detalle.cantidad;
                existente.precio_unitario = detalle.precio_unitario;

                return context.SaveChanges();
            }
        }

        /// <summary>
        /// Elimina físicamente un DETALLE_PEDIDO (o podrías hacer un borrado lógico si agregaras un campo estado_registro).
        /// </summary>
        public static int Eliminar(int id)
        {
            using (var context = new LabPizzeriaEntities())
            {
                var det = context.DETALLE_PEDIDO.Find(id);
                if (det == null) return 0;

                // Aquí quitamos el registro de la BD
                context.DETALLE_PEDIDO.Remove(det);
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// Devuelve un DETALLE_PEDIDO dado su ID.
        /// </summary>
        public static DETALLE_PEDIDO ObtenerUno(int id)
        {
            using (var context = new LabPizzeriaEntities())
            {
                return context.DETALLE_PEDIDO.Find(id);
            }
        }

        /// <summary>
        /// Lista todos los DETALLE_PEDIDO.
        /// Si tuvieras campo de “estado_registro” en la entidad, filtra por estado = true.
        /// En este modelo en particular, no aparece esa propiedad, así que listamos todo:
        /// </summary>
        public static List<DETALLE_PEDIDO> Listar()
        {
            using (var context = new LabPizzeriaEntities())
            {
                return context.DETALLE_PEDIDO.ToList();
            }
        }
    }
}
