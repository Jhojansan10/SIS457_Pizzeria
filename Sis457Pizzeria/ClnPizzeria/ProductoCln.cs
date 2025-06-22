using System.Collections.Generic;
using CadPizzeria;

namespace ClnPizzeria
{
    public class ProductoCln
    {

        public static void Insertar(Producto producto)
        {
            ProductoCad.Insertar(producto);
        }

        public static Producto Obtener(int id)
        {
            return ProductoCad.Obtener(id);
        }

        public static void Actualizar(Producto producto)
        {
            ProductoCad.Actualizar(producto);
        }

        public static void Eliminar(int id)
        {
            ProductoCad.Eliminar(id);
        }

        public static List<Producto> Listar(string filtro)
        {
            if (string.IsNullOrWhiteSpace(filtro))
                return ProductoCad.Listar();
            else
                return ProductoCad.Listar(filtro);
        }



    }
}
