using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CadPizzeria;

namespace ClnPizzeria
{
    public class RepartidorCln
    {
        public static int Insertar(REPARTIDOR rep)
        {
            using (var context = new LabPizzeriaEntities())
            {
                context.REPARTIDOR.Add(rep);
                context.SaveChanges();
                return rep.usuario_id; // En tu modelo, usuario_id es PK
            }
        }

        public static int Actualizar(REPARTIDOR rep)
        {
            using (var context = new LabPizzeriaEntities())
            {
                var existente = context.REPARTIDOR.Find(rep.usuario_id);
                if (existente == null) return 0;

                existente.nro_licencia = rep.nro_licencia;
                existente.fecha_ingreso = rep.fecha_ingreso;
                existente.estado_registro = rep.estado_registro;

                return context.SaveChanges();
            }
        }

        public static int Eliminar(int id)
        {
            using (var context = new LabPizzeriaEntities())
            {
                var r = context.REPARTIDOR.Find(id);
                if (r == null) return 0;

                r.estado_registro = false;
                return context.SaveChanges();
            }
        }

        public static REPARTIDOR ObtenerUno(int id)
        {
            using (var context = new LabPizzeriaEntities())
            {
                return context.REPARTIDOR.Find(id);
            }
        }

        public static List<REPARTIDOR> Listar()
        {
            using (var context = new LabPizzeriaEntities())
            {
                return context.REPARTIDOR
                              .Where(x => x.estado_registro == true)
                              .ToList();
            }
        }

        // Agrega métodos ListarPa… si importas SP de repartidor.
    }
}
