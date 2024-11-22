using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmlyWeb.Models;
using FarmlyWeb.Extensions;

namespace FarmlyWeb.Controllers
{
    public class ProdutoController : BaseController
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
        [HttpPost]
        public IActionResult AdicionarAoCarrinho(int idProduto, int quantidade)
        {
            var produto = _context.Produto.FirstOrDefault(p => p.Id == idProduto);

            if (produto == null)
            {
                TempData["MensagemErro"] = "Produto não encontrado.";
                return RedirectToAction("Index", "Produto");
            }

            // Verifica se a quantidade solicitada está disponível no estoque
            if (quantidade > produto.QuantidadeEstoque)
            {
                TempData["MensagemErro"] = $"Quantidade solicitada ({quantidade}) excede o estoque disponível ({produto.QuantidadeEstoque}).";
                return RedirectToAction("Index", "Produto");
            }

            // Lista local para o carrinho (simulando uma sessão ou armazenamento em memória)
            var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();

            // Verifica se o produto já existe no carrinho
            var itemExistente = carrinho.FirstOrDefault(c => c.ProdutoId == idProduto);
            if (itemExistente != null)
            {
                // Atualiza a quantidade do item existente
                itemExistente.Quantidade += quantidade;
            }
            else
            {
                // Adiciona um novo item ao carrinho
                carrinho.Add(new CarrinhoItem
                {
                    ProdutoId = produto.Id,
                    Nome = produto.Nome,
                    Quantidade = quantidade,
                    PrecoUnitario = produto.Preco
                });
            }

            // Atualiza o estoque do produto
            produto.QuantidadeEstoque -= quantidade;

            // Salva o carrinho na sessão
            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);

            // Salva as alterações no banco de dados
            _context.SaveChanges();

            TempData["MensagemSucesso"] = "Produto adicionado ao carrinho com sucesso!";
            return RedirectToAction("Index", "Produto");
        }

    }
}
