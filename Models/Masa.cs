using System.Collections.Generic;

namespace sis457_pizzeria.Web.Models
{
    public class Masa
    {
        public int MasaId { get; set; }
        public string Tipo { get; set; }
        public decimal MultiplicadorPrecio { get; set; }

        public ICollection<Pizza> Pizzas { get; set; }
    }
}
