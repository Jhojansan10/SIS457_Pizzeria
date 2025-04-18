namespace sis457_pizzeria.Web.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Rol { get; set; }     // e.g. Admin, Empleado
    }
}
