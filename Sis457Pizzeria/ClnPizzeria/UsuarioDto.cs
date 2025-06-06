using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClnPizzeria
{
    public class UsuarioDto
    {
        public int usuario_id { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string contraseña { get; set; }
        public string rol { get; set; }
        public DateTime fecha_registro { get; set; }
        public bool estado { get; set; }
    }
}
