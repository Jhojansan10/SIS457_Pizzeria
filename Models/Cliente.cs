using System.Collections.Generic;

using System.Collections.Generic;

namespace sis457_pizzeria.Web.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        public ICollection<Pedido> Pedidos { get; set; }
    }
}
