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

    // Adicionar um item ao carrinho
    [HttpPost]
    public IActionResult AdicionarItem(int id, int quantidade)
    {
        var produto = _context.Produto.Find(id);
        if (produto == null || quantidade <= 0)
            return NotFound();

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
        return RedirectToAction("Index", "Produto");
    }

    // Método para exibir o carrinho
    public IActionResult Carrinho()
    {
        var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();
        return View(carrinho);
    }

    // Método para remover item do carrinho
    [HttpPost]
    public IActionResult RemoverItem(int id)
    {
        var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();
        var item = carrinho.FirstOrDefault(p => p.ProdutoId == id);

        if (item != null)
        {
            carrinho.Remove(item);
        }

        HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
        return RedirectToAction("Carrinho"); // Redireciona para a página do carrinho
    }

    // Outros métodos permanecem inalterados
}
