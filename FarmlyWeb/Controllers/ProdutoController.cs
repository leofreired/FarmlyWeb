using System;
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

        // GET: Produto
        public async Task<IActionResult> Index()
        {
            return View(await _context.Produto.ToListAsync());
        }

        // GET: Produto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
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

        // POST: Produto/AdicionarAoCarrinho/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarAoCarrinho(int id, int quantidade)
        {
            if (quantidade <= 0)
            {
                return BadRequest("A quantidade deve ser maior que zero.");
            }

            var produto = _context.Produto.Find(id);
            if (produto == null)
            {
                return NotFound();
            }

            // Adicionar item ao carrinho
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
