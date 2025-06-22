using System.Collections.Generic;
using CadPizzeria;

namespace ClnPizzeria
{
    public class ClienteCln
    {
        public static List<Cliente> Listar(string filtro)
        {
            return ClienteCad.Listar(filtro);
        }

        public static void Insertar(Cliente cliente)
        {
            ClienteCad.Insertar(cliente);
        }

        public static Cliente Obtener(int id)
        {
            return ClienteCad.Obtener(id);
        }

        public static void Actualizar(Cliente cliente)
        {
            ClienteCad.Actualizar(cliente);
        }

        public static void Eliminar(int id)
        {
            ClienteCad.Eliminar(id);
        }
    }
}
