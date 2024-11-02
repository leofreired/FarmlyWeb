using Microsoft.AspNetCore.Mvc;
using FarmlyWeb.Models;
using System.Collections.Generic;
using System.Linq;
using FarmlyWeb.Extensions;

public class CarrinhoController : Controller
{
    private readonly Contexto _context;

    public CarrinhoController(Contexto context)
    {
        _context = context;
    }

    // Exibir o carrinho
    public IActionResult Carrinho()
    {
        if (!HttpContext.Session.GetInt32("ClienteId").HasValue)
        {
            return RedirectToAction("Index", "Login");
        }

        var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();
        ViewBag.Total = carrinho.Sum(item => item.Total);
        return View(carrinho);
    }

    // Adicionar item ao carrinho com verificação de estoque
    [HttpPost]
    public IActionResult AdicionarItem(int id, int quantidade)
    {
        if (!HttpContext.Session.GetInt32("ClienteId").HasValue)
        {
            return RedirectToAction("Index", "Login");
        }

        var produto = _context.Produto.Find(id);
        if (produto == null)
            return NotFound();

        if (quantidade <= 0)
            return RedirectToAction("Index", "Produto", new { mensagem = "Quantidade inválida!" });

        if (produto.QuantidadeEstoque < quantidade)
        {
            TempData["Erro"] = $"Não há estoque suficiente para {produto.Nome}. Quantidade disponível: {produto.QuantidadeEstoque}.";
            return RedirectToAction("Index", "Produto");
        }

        var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();

        var itemExistente = carrinho.FirstOrDefault(p => p.ProdutoId == id);
        if (itemExistente != null)
        {
            int novaQuantidade = itemExistente.Quantidade + quantidade;
            if (novaQuantidade > produto.QuantidadeEstoque)
            {
                TempData["Erro"] = $"Estoque insuficiente para adicionar {novaQuantidade} unidades de {produto.Nome}. Quantidade disponível: {produto.QuantidadeEstoque}.";
                return RedirectToAction("Index", "Produto");
            }
            itemExistente.Quantidade = novaQuantidade;
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
        return RedirectToAction("Index", "Produto");
    }
}
