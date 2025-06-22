using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebPizzeria.Filters
{
    public class AuthorizeByRoleAttribute : ActionFilterAttribute
    {
        private readonly string[] _rolesPermitidos;

        public AuthorizeByRoleAttribute(params string[] roles)
        {
            _rolesPermitidos = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var rol = context.HttpContext.Session.GetString("UsuarioRol");
            if (string.IsNullOrEmpty(rol) || !_rolesPermitidos.Contains(rol))
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "controller", "Login" },
                        { "action", "Empleado" }
                    });
            }
        }
    }
}
