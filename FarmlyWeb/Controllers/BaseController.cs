﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class BaseController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        //rotas nao autenticadas
        var naoAutenticadas = new List <string>() {"Login", "Cliente"};

        base.OnActionExecuting(context);

        var rotaAtual = context.RouteData.Values["controller"]?.ToString();

        if (!naoAutenticadas.Contains(rotaAtual))
        {
            if (HttpContext.Session.GetInt32("ClienteId") == null)
            {
                // Redireciona para o login
                context.Result = RedirectToAction("Index", "Login");
            }
        }
    }
    
}
