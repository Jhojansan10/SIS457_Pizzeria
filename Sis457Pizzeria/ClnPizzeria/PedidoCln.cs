using CadPizzeria;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClnPizzeria
{
    public class PedidoCln
    {
        public static int insertar(PEDIDO pedido, List<DETALLE_PEDIDO> detalles)
        {
            using (var db = new LabPizzeriaEntities())
            {
                pedido.estado = "Pendiente";
                pedido.estado_registro = true;
                db.PEDIDO.Add(pedido);
                db.SaveChanges();

                foreach (var detalle in detalles)
                {
                    detalle.pedido_id = pedido.pedido_id;
                    db.DETALLE_PEDIDO.Add(detalle);
                }

                db.SaveChanges();
                return pedido.pedido_id;
            }
        }

        public static List<object> listar()
        {
            using (var db = new LabPizzeriaEntities())
            {
                return db.PEDIDO
                    .Where(p => p.estado_registro == true)
                    .OrderByDescending(p => p.fecha)
                    .Select(p => new
                    {
                        p.pedido_id,
                        Cliente = p.USUARIO.nombre,
                        Direccion = p.DIRECCION.calle,
                        p.fecha,
                        Estado = p.estado_registro, // Ajustar si tienes un campo tipo string para estado
                        p.total
                    })
                    .ToList<object>();
            }
        }
    }
}
