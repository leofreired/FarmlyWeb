// CarrinhoController.cs
using Microsoft.AspNetCore.Mvc;
using FarmlyWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using FarmlyWeb.Extensions;

public class CarrinhoController : Controller
{
    private readonly DbContext _context;

    public CarrinhoController(DbContext context)
    {
        _context = context;
    }

    // Exibir lista de produtos para clientes
    public IActionResult Index()
    {
        var produtos = _context.Set<Produto>().ToList();
        return View(produtos);
    }

    // Adicionar um item ao carrinho (pode ser armazenado na sessão)
    [HttpPost]
    public IActionResult AdicionarItem(int id, int quantidade)
    {
        var produto = _context.Set<Produto>().Find(id);
        if (produto == null || quantidade <= 0)
            return NotFound();

        // Recuperar o carrinho da sessão
        var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();

        // Verificar se o produto já está no carrinho
        var itemExistente = carrinho.FirstOrDefault(p => p.ProdutoId == id);
        if (itemExistente != null)
        {
            // Atualizar a quantidade se o item já existe
            itemExistente.Quantidade += quantidade;
        }
        else
        {
            // Adicionar um novo item ao carrinho
            carrinho.Add(new CarrinhoItem
            {
                ProdutoId = produto.Id,
                Nome = produto.Nome,
                Quantidade = quantidade,
                PrecoUnitario = produto.Preco
            });
        }

        // Salvar o carrinho atualizado na sessão
        HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);

        // Redirecionar de volta para a página de produtos disponíveis
        return RedirectToAction("Index");
    }

    // Exibir carrinho
    public IActionResult Carrinho()
    {
        // Recuperar itens do carrinho da sessão
        var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho") ?? new List<CarrinhoItem>();
        return View(carrinho); // Passar os itens do carrinho para a view
    }

    // Finalizar Compra
    [HttpPost]
    public IActionResult FinalizarCompra()
    {
        // Recuperar itens do carrinho da sessão
        var carrinho = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho");
        if (carrinho == null || !carrinho.Any())
        {
            // Redirecionar de volta ao carrinho se estiver vazio
            return RedirectToAction("Carrinho");
        }

        // Criar objeto Venda e VendaItens, salvar no banco de dados
        var venda = new Venda
        {
            IdCliente = 1, // Defina o cliente autenticado ou a lógica para obter o cliente
            DataVenda = DateTime.Now,
            Preco = carrinho.Sum(i => i.Total),
            Pagamento = 1, // Suponha que seja pago por agora
            Status = 1 // Suponha que seja aprovado por agora
        };

        _context.Set<Venda>().Add(venda);
        _context.SaveChanges();

        // Criar itens da venda
        foreach (var item in carrinho)
        {
            var vendaItem = new VendaItens
            {
                IdVenda = venda.Id,
                IdProduto = item.ProdutoId,
                Quantidade = item.Quantidade,
                Preco = item.PrecoUnitario
            };
            _context.Set<VendaItens>().Add(vendaItem);
        }

        _context.SaveChanges();

        // Limpar o carrinho da sessão
        HttpContext.Session.Remove("Carrinho");

        return RedirectToAction("Confirmacao");
    }

    // Página de confirmação
    public IActionResult Confirmacao()
    {
        return View();
    }
}
