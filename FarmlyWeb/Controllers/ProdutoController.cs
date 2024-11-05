using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmlyWeb.Models;
using FarmlyWeb.Extensions;

namespace FarmlyWeb.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly Contexto _context;

        public ProdutoController(Contexto context)
        {
            _context = context;
        }

        // Exibir produtos com funcionalidade de busca
        public async Task<IActionResult> Index(string searchQuery)
        {
            if (!HttpContext.Session.GetInt32("ClienteId").HasValue)
            {
                return RedirectToAction("Index", "Login");
            }

            var produtos = string.IsNullOrWhiteSpace(searchQuery)
                ? await _context.Produto.ToListAsync()
                : await _context.Produto.Where(p => p.Nome.Contains(searchQuery)).ToListAsync();

            ViewBag.SearchQuery = searchQuery; // Para exibir o texto digitado na busca
            return View(produtos);
        }

        // Exibir detalhes de um produto específico
        public async Task<IActionResult> Details(int? id)
        {
            if (!HttpContext.Session.GetInt32("ClienteId").HasValue)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return NotFound();
            }

            var produto = await _context.Produto.FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }

        // Adicionar item ao carrinho
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarAoCarrinho(int id, int quantidade)
        {
            if (!HttpContext.Session.GetInt32("ClienteId").HasValue)
            {
                return RedirectToAction("Index", "Login");
            }

            if (quantidade <= 0)
            {
                return BadRequest("A quantidade deve ser maior que zero.");
            }

            var produto = _context.Produto.Find(id);
            if (produto == null)
            {
                return NotFound();
            }

            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();

            var itemExistente = carrinho.FirstOrDefault(p => p.ProdutoId == id);
            if (itemExistente != null)
            {
                itemExistente.Quantidade += quantidade;
            }
            else
            {
                carrinho.Add(new CarrinhoItem
                {
                    ProdutoId = produto.Id,
                    Nome = produto.Nome,
                    Quantidade = quantidade,
                    PrecoUnitario = produto.Preco
                });
            }

            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
            return RedirectToAction("Index");
        }
    }
}
