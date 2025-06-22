using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace CadPizzeria
{
    public class PedidoCad
    {
        public static List<Pedido> Listar()
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Pedido
                    .Include(p => p.Cliente)
                    .Where(p => p.estado != -1)
                    .OrderByDescending(p => p.fechaPedido)
                    .ToList();
            }
        }

        public static Pedido Obtener(int id)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.Pedido
                    .Include(p => p.Cliente)
                    .Include(p => p.Direccion)
                    .Include(p => p.Empleado)
                    .Include(p => p.DetallePedido.Select(d => d.Producto))
                    .FirstOrDefault(p => p.id == id && p.estado != -1);
            }
        }


        public static void Insertar(Pedido pedido, List<DetallePedido> detalles)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        pedido.fechaPedido = DateTime.Now;
                        pedido.estado = 1;
                        ctx.Pedido.Add(pedido);
                        ctx.SaveChanges();

                        decimal totalPedido = 0m;

                        foreach (var detalle in detalles)
                        {
                            // Validación de stock
                            var producto = ctx.Producto.Find(detalle.idProducto);
                            if (producto == null || producto.stock < detalle.cantidad)
                                throw new Exception("Stock insuficiente para el producto seleccionado.");

                            // Descontar stock
                            producto.stock -= detalle.cantidad;

                            // Calcular subtotal
                            detalle.idPedido = pedido.id;
                            detalle.precioUnitario = producto.precioVenta;
                            detalle.total = detalle.precioUnitario * detalle.cantidad;
                            detalle.estado = 1;

                            totalPedido += detalle.total;
                            ctx.DetallePedido.Add(detalle);
                        }

                        // Actualizar total del pedido
                        pedido.total = totalPedido;
                        ctx.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
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
            using (var ctx = new FinalPizzeriaEntities())
            {
                var pedido = ctx.Pedido.Find(id);
                if (pedido != null)
                {
                    pedido.estado = -1;
                    ctx.SaveChanges();
                }
            }
        }
    }
}
