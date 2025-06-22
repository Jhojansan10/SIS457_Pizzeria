using System.Collections.Generic;
using CadPizzeria;

namespace ClnPizzeria
{
    public class PagoCln
    {
        public static List<Pago> Listar(string filtro)
        {
            return PagoCad.Listar(filtro);
        }

        public static void Insertar(Pago pago)
        {
            PagoCad.Insertar(pago);
        }

        public static Pago Obtener(int id)
        {
            return PagoCad.Obtener(id);
        }

        public static void Actualizar(Pago pago)
        {
            PagoCad.Actualizar(pago);
        }

        public static void Eliminar(int id)
        {
            PagoCad.Eliminar(id);
        }
    }
}
