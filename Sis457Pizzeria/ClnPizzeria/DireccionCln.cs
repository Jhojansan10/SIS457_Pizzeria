using System.Collections.Generic;
using CadPizzeria;

namespace ClnPizzeria
{
    public class DireccionCln
    {
        public static List<Direccion> Listar()
        {
            return DireccionCad.Listar();
        }

        public static void Insertar(Direccion direccion)
        {
            DireccionCad.Insertar(direccion);
        }

        public static Direccion Obtener(int id)
        {
            return DireccionCad.Obtener(id);
        }

        public static void Actualizar(Direccion direccion)
        {
            DireccionCad.Actualizar(direccion);
        }

        public static void Eliminar(int id)
        {
            DireccionCad.Eliminar(id);
        }
    }
}
