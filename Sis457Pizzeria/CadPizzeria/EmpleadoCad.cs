using System;
using System.Collections.Generic;
using System.Linq;

namespace CadPizzeria
{
    public class EmpleadoCad
    {
        public static List<Empleado> Listar(string filtro)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Empleado
                    .Where(e => e.estado != -1 &&
                        (e.cedulaIdentidad.Contains(filtro) ||
                         e.nombres.Contains(filtro) ||
                         e.primerApellido.Contains(filtro) ||
                         e.segundoApellido.Contains(filtro)))
                    .OrderBy(e => e.nombres)
                    .ToList();
            }
        }

        public static Empleado Obtener(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Empleado.FirstOrDefault(e => e.id == id && e.estado != -1);
            }
        }

        public static void Insertar(Empleado empleado)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                empleado.fechaRegistro = DateTime.Now;
                empleado.estado = 1;
                ctx.Empleado.Add(empleado);
                ctx.SaveChanges();
            }
        }

        public static void Modificar(Empleado empleado)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var original = ctx.Empleado.FirstOrDefault(e => e.id == empleado.id);
                if (original != null)
                {
                    original.cedulaIdentidad = empleado.cedulaIdentidad;
                    original.nombres = empleado.nombres;
                    original.primerApellido = empleado.primerApellido;
                    original.segundoApellido = empleado.segundoApellido;
                    original.direccion = empleado.direccion;
                    original.celular = empleado.celular;
                    original.cargo = empleado.cargo;
                    ctx.SaveChanges();
                }
            }
        }

        public static void Eliminar(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                var empleado = ctx.Empleado.FirstOrDefault(e => e.id == id);
                if (empleado != null)
                {
                    empleado.estado = -1;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
