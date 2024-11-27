using FarmlyWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class LoginController : BaseController
{
    private readonly Contexto _context;

    public LoginController(Contexto context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    // GET: Cliente/Create
    public IActionResult Create()
    {
        return View("Create");
    }

    [HttpPost]
    public IActionResult Index(string cpf, string senha)
    {
        // Busca o cliente no banco de dados pelo CPF e senha
        var cliente = _context.Cliente.FirstOrDefault(c => c.Cpf == cpf && c.Senha == senha);

        if (cliente != null)
        {
            // Armazena informações na sessão
            HttpContext.Session.SetInt32("ClienteId", cliente.Id);
            HttpContext.Session.SetString("ClienteNome", cliente.Nome);
            HttpContext.Session.SetString("ClienteCPF", cliente.Cpf);

            return RedirectToAction("Index", "Home");
        }

        // Caso falhe, exibe uma mensagenzinha de erro (é para aparecer)
        ViewBag.Erro = "CPF ou senha inválidos!";
        return View();
    }

    public IActionResult Logout()
    {
        // Limpa a sessão ao sair
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}
