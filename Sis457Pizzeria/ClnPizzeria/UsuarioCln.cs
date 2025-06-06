using CadPizzeria;
using System.Linq;
using System;
using System.Collections.Generic;

namespace ClnPizzeria
{
    public class UsuarioCln
    {
        public static USUARIO validar(string email, string clave)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var lista = db.USUARIO.ToList();
                Console.WriteLine("Usuarios encontrados: " + lista.Count);


                foreach (var u in lista)
                {
                    Console.WriteLine("Comparando con:");
                    Console.WriteLine("Email: " + u.email);
                    Console.WriteLine("Contraseña (hash): " + u.contraseña);
                    Console.WriteLine("Estado: " + u.estado);

                    if (u.email.Trim().ToLower() == email.Trim().ToLower()
                        && u.contraseña == clave
                        && u.estado == true)

                        Console.WriteLine("Comparando:");
                    Console.WriteLine("  Email BD : " + u.email.Trim());
                    Console.WriteLine("  Email UI : " + email.Trim());
                    Console.WriteLine("  Clave BD : " + u.contraseña);
                    Console.WriteLine("  Clave UI : " + clave);
                    Console.WriteLine("  Estado   : " + u.estado);
                    Console.WriteLine("  Coinciden: " +
                        (u.email.Trim() == email.Trim()) + " | " +
                        (u.contraseña == clave) + " | " +
                        (u.estado == true));


                    {
                        return u;
                    }
                }

                return null;
            }
        }

        public static List<UsuarioDto> listar()
        {
            using (var db = new LabPizzeriaEntities())
            {
                return db.USUARIO
                    .Where(u => u.estado == true)
                    .OrderBy(u => u.nombre)
                    .Select(u => new UsuarioDto
                    {
                        usuario_id = u.usuario_id,
                        nombre = u.nombre,
                        email = u.email,
                        contraseña = u.contraseña,
                        rol = u.rol,
                        fecha_registro = u.fecha_registro,
                        estado = u.estado
                    })
                    .ToList();
            }
        }


        public static int insertar(USUARIO usuario)
        {
            using (var db = new LabPizzeriaEntities())
            {
                if (db.USUARIO.Any(u => u.email == usuario.email && u.estado == true))
                    throw new Exception("Ya existe un usuario con este correo.");

                if (db.USUARIO.Any(u => u.email == usuario.email && u.usuario_id != usuario.usuario_id && u.estado == true))
                    throw new Exception("Ya existe otro usuario con este correo.");

                usuario.fecha_registro = DateTime.Now;
                usuario.estado = true;
                db.USUARIO.Add(usuario);
                db.SaveChanges();
                return usuario.usuario_id;

            }

        }

        public static void actualizar(USUARIO usuario)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var original = db.USUARIO.Find(usuario.usuario_id);
                original.nombre = usuario.nombre;
                original.email = usuario.email;
                original.contraseña = usuario.contraseña;
                original.rol = usuario.rol;
                db.SaveChanges();
            }
        }

        public static void eliminar(int id)
        {
            using (var db = new LabPizzeriaEntities())
            {
                var usuario = db.USUARIO.Find(id);
                usuario.estado = false;
                db.SaveChanges();
            }
        }

    }
}
