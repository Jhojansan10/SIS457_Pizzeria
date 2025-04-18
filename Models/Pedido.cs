using System;
using System.Collections.Generic;

namespace sis457_pizzeria.Web.Models
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }

        public Cliente Cliente { get; set; }
        public ICollection<Pizza> Pizzas { get; set; }
    }
}
