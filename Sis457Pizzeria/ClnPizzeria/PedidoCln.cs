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
            using (var ctx = new FinalPizzeriaEntities())
            {
                var original = ctx.Pedido.Find(pedido.id);
                if (original != null)
                {
                    original.estadoEntrega = pedido.estadoEntrega;
                    ctx.SaveChanges();
                }
            }
        }



        public static void Eliminar(int id)
        {
            PedidoCad.Eliminar(id);
        }
    }
}
