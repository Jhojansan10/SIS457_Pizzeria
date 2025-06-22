using System.Collections.Generic;
using CadPizzeria;

namespace ClnPizzeria
{
    public class PedidoCln
    {
        public static List<Pedido> Listar()
        {
            return PedidoCad.Listar();
        }

        public static Pedido Obtener(int id)
        {
            return PedidoCad.Obtener(id);
        }

        public static void Insertar(Pedido pedido, List<DetallePedido> detalles)
        {
            PedidoCad.Insertar(pedido, detalles);
        }

        public static void Actualizar(Pedido pedido)
        {
            PedidoCad.Actualizar(pedido);
        }


        public static void Eliminar(int id)
        {
            PedidoCad.Eliminar(id);
        }
    }
}
