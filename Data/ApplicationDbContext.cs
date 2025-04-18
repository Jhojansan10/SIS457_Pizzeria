using Microsoft.EntityFrameworkCore;
using sis457_pizzeria.Web.Models;

namespace sis457_pizzeria.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Ingrediente> Ingredientes { get; set; }
        public DbSet<PizzaIngrediente> PizzaIngredientes { get; set; }
        public DbSet<Tamano> Tamanos { get; set; }
        public DbSet<Masa> Masas { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
