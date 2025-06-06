using CadPizzeria;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClnPizzeria
{
    public class PagoCln
    {
        public static List<object> listar(string criterio = "")
        {
            using (var db = new LabPizzeriaEntities())
            {
                return db.PAGO
                    .Where(p => p.PEDIDO.USUARIO.nombre.Contains(criterio)
                             || p.pedido_id.ToString().Contains(criterio)
                             || p.metodo.Contains(criterio))
                    .OrderByDescending(p => p.fecha_pago)
                    .Select(p => new
                    {
                        p.pago_id,
                        Cliente = p.PEDIDO.USUARIO.nombre,
                        Pedido = p.pedido_id,
                        Monto = p.monto_pagado,
                        Metodo = p.metodo,
                        Fecha = p.fecha_pago
                    }).ToList<object>();
            }
        }

        public static int insertar(PAGO pago)
        {
            using (var db = new LabPizzeriaEntities())
            {
                db.PAGO.Add(pago);
                db.SaveChanges();
                return pago.pago_id;
            }
        }

        public static void actualizar(PAGO pago)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var original = db.PAGO.Find(pago.pago_id);
                original.pedido_id = pago.pedido_id;
                original.monto_pagado = pago.monto_pagado;
                original.metodo = pago.metodo;
                original.fecha_pago = pago.fecha_pago;
                db.SaveChanges();
            }
        }

        public static void eliminar(int id)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var pago = db.PAGO.Find(id);
                if (pago != null)
                {
                    db.PAGO.Remove(pago);
                    db.SaveChanges();
                }
            }
        }
    }

}
