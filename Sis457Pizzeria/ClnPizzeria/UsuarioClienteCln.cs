using System.Collections.Generic;
using CadPizzeria;

namespace ClnPizzeria
{
    public class UsuarioClienteCln
    {
        public static void Insertar(UsuarioCliente usuarioCliente)
        {
            UsuarioClienteCad.Insertar(usuarioCliente);
        }

        public static void Modificar(UsuarioCliente usuarioCliente)
        {
            UsuarioClienteCad.Modificar(usuarioCliente);
        }

        public static void Eliminar(int id)
        {
            UsuarioClienteCad.Eliminar(id);
        }

        public static UsuarioCliente Obtener(int id)
        {
            return UsuarioClienteCad.Obtener(id);
        }

        public static List<UsuarioCliente> Listar(string filtro)
        {
            return UsuarioClienteCad.Listar(filtro);
        }
    }
}

