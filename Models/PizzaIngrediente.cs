namespace sis457_pizzeria.Web.Models
{
    public class PizzaIngrediente
    {
        public int PizzaIngredienteId { get; set; }
        public int PizzaId { get; set; }
        public int IngredienteId { get; set; }

        public Pizza Pizza { get; set; }
        public Ingrediente Ingrediente { get; set; }
    }
}
