using System.Collections.Generic;

namespace sis457_pizzeria.Web.Models
{
    public class Tamano
    {
        public int TamanoId { get; set; }
        public string Descripcion { get; set; }       // e.g. Pequeña, Mediana, Grande
        public decimal MultiplicadorPrecio { get; set; }

        public ICollection<Pizza> Pizzas { get; set; }
    }
}
