using Microsoft.AspNetCore.Mvc;
using FarmlyWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FarmlyWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly Contexto _context;

        public LoginController(Contexto context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Index(string cpf, string senha)
        {
            var cliente = await _context.Cliente
                .FirstOrDefaultAsync(c => c.Cpf == cpf && c.Senha == senha);

            if (cliente != null)
            {
                HttpContext.Session.SetInt32("ClienteId", cliente.Id);  // Salva o Id do cliente na sessão
                return RedirectToAction("Index", "Produto");
            }

            ViewBag.ErrorMessage = "CPF ou senha incorretos.";
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("ClienteId");
            return RedirectToAction("Index", "Login");
        }
    }
}
