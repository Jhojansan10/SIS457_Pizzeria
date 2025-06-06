using System.Security.Cryptography;
using System.Text;
using CadPizzeria; // Para poder usar la clase USUARIO

namespace CpPizzeria
{
    public class Util
    {
        public static string Encrypt(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }

        // Aquí se guarda el usuario que inició sesión
        public static USUARIO usuario;
    }
}
