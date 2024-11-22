using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        base.OnActionExecuting(context);

        // Verifica se o cliente está logado
        if (HttpContext.Session.GetInt32("ClienteId") == null &&
            !(context.RouteData.Values["controller"]?.ToString() == "Login" &&
              context.RouteData.Values["action"]?.ToString() == "Index"))
        {
            // Redireciona para o login apenas se não for a página de login
            context.Result = RedirectToAction("Index", "Login");
        }
    }
}
