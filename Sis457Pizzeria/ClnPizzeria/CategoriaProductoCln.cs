using CadPizzeria;
using System.Collections.Generic;
using System.Linq;

namespace ClnPizzeria
{
    public class CategoriaProductoCln
    {
        public static List<CategoriaProducto> Listar(string filtro)
        {
            using (var ctx = new FinalPizzeriaEntities())
            {
                return ctx.CategoriaProducto
                    .Where(c => c.estado != -1 && (c.nombre.Contains(filtro) || filtro == ""))
                    .OrderBy(c => c.nombre)
                    .ToList();
            }
        }

        public static CategoriaProducto Obtener(int id)
        {
            return CategoriaProductoCad.Obtener(id);
        }

        public static void Insertar(CategoriaProducto categoria)
        {
            CategoriaProductoCad.Insertar(categoria);
        }

        public static void Actualizar(CategoriaProducto categoria)
        {
            CategoriaProductoCad.Actualizar(categoria);
        }

        public static void Eliminar(int id)
        {
            CategoriaProductoCad.Eliminar(id);
        }
    }
}
