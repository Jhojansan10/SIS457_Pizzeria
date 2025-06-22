using System;
using System.Collections.Generic;
using CadPizzeria;

namespace ClnPizzeria
{
    public class UsuarioCln
    {
        public static List<Usuario> Listar(string filtro)
        {
            return UsuarioCad.Listar(filtro);
        }

        public static Usuario Obtener(int id)
        {
            return UsuarioCad.Obtener(id);
        }

        public static void Insertar(Usuario usuario)
        {
            UsuarioCad.Insertar(usuario);
        }

        public static void Modificar(Usuario usuario)
        {
            UsuarioCad.Modificar(usuario);
        }

        public static void Eliminar(int id)
        {
            UsuarioCad.Eliminar(id);
        }

        public static bool ExisteEmpleadoConUsuario(int idEmpleado)
        {
            return UsuarioCad.ExisteEmpleadoConUsuario(idEmpleado);
        }
    }
}
