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

    // Método para exibir o carrinho
    public IActionResult Carrinho()
    {
        // Recupera o carrinho da sessão
        var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();

        // Calcula o total da compra
        decimal total = carrinho.Sum(item => item.Total);

        // Passa o carrinho e o total para a view
        ViewBag.Total = total;
        return View(carrinho);
    }

    // Adicionar um item ao carrinho com verificação de estoque
    [HttpPost]
    public IActionResult AdicionarItem(int id, int quantidade)
    {
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

    // Método para remover uma quantidade específica de um item do carrinho
    [HttpPost]
    public IActionResult RemoverItem(int id, int quantidadeRemover)
    {
        var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();

        var item = carrinho.FirstOrDefault(p => p.ProdutoId == id);
        if (item != null)
        {
            if (quantidadeRemover >= item.Quantidade)
            {
                // Remove o item completamente se a quantidade a remover é igual ou maior que a quantidade no carrinho
                carrinho.Remove(item);
            }
            else
            {
                // Caso contrário, apenas diminui a quantidade
                item.Quantidade -= quantidadeRemover;
            }

            // Atualiza o carrinho na sessão
            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
        }

        return RedirectToAction("Carrinho");
    }
}
