using System.Collections.Generic;
using CadPizzeria;

namespace ClnPizzeria
{
    public class EmpleadoCln
    {
        public static List<Empleado> Listar(string filtro)
        {
            return EmpleadoCad.Listar(filtro);
        }

        public static Empleado Obtener(int id)
        {
            return EmpleadoCad.Obtener(id);
        }

        public static void Insertar(Empleado empleado)
        {
            EmpleadoCad.Insertar(empleado);
        }

        public static void Modificar(Empleado empleado)
        {
            EmpleadoCad.Modificar(empleado);
        }

        public static void Eliminar(int id)
        {
            EmpleadoCad.Eliminar(id);
        }
    }
}
