using System.Collections.Generic;

namespace sis457_pizzeria.Web.Models
{
    public class Ingrediente
    {
        public int IngredienteId { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioAdicional { get; set; }
        public int Stock { get; set; }

        public ICollection<PizzaIngrediente> PizzaIngredientes { get; set; }
    }
}
