using System.Collections.Generic;

namespace sis457_pizzeria.Web.Models
{
    public class Pizza
    {
        public int PizzaId { get; set; }
        public string Nombre { get; set; }
        public int TamanoId { get; set; }
        public int MasaId { get; set; }
        public decimal PrecioBase { get; set; }

        public Tamano Tamano { get; set; }
        public Masa Masa { get; set; }
        public ICollection<PizzaIngrediente> PizzaIngredientes { get; set; }
    }
}
