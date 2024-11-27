using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmlyWeb.Models;
using Newtonsoft.Json;
using FarmlyWeb.DTOs;
using System.Collections.Generic;
using FarmlyWeb.Extensions;

namespace FarmlyWeb.Controllers
{
    public class VendaController : BaseController
    {
        private readonly Contexto _context;
        private readonly HttpClient _httpClient;

        public VendaController(Contexto context)
        {
            _context = context;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7186/")
            };
        }

        // GET: Venda
        public async Task<IActionResult> Index()
        {
            return View(await _context.Venda.ToListAsync());
        }

        // GET: Venda/FinalizarCompra
        public IActionResult FinalizarCompra()
        {
            var clienteId = HttpContext.Session.GetInt32("ClienteId");

            if (clienteId == null || clienteId == 0)
            {
                // Redireciona para o login se o cliente não estiver autenticado
                return RedirectToAction("Index", "Login");
            }

            var meusItens = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho");

            if (meusItens == null || !meusItens.Any())
            {
                ViewData["ErrorMessage"] = "O carrinho está vazio.";
                return RedirectToAction("Index", "Carrinho");
            }

            var soma = meusItens.Sum(x => x.Quantidade * x.PrecoUnitario);

            var venda = new Venda
            {
                IdCliente = clienteId.Value,
                DataVenda = DateTime.Now,
                Preco = soma,
                Pagamento = string.Empty,
                Status = "0" // Status inicial como Pendente
            };

            Console.WriteLine($"ClienteId: {venda.IdCliente}");
            return View(venda);
        }

        public IActionResult Sucesso()
        {
            return View();
        }

        // POST: Venda/FinalizarCompra
        [HttpPost]
        public async Task<IActionResult> ConfirmarCompra(Venda venda)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("FinalizarCompra", venda);
                }

                // 1. Criar a venda na API
                var vendaDto = new VendaDTO
                {
                    IdCliente = venda.IdCliente,
                    DataVenda = venda.DataVenda,
                    PrecoTotal = venda.Preco,
                    Pagamento = venda.Pagamento,
                    Status = venda.Status
                };

                var vendaJson = JsonConvert.SerializeObject(vendaDto);
                var response = await _httpClient.PostAsync("api/venda",
                    new StringContent(vendaJson, Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    ViewData["ErrorMessage"] = "Erro ao criar a venda.";
                    return View("FinalizarCompra", venda);
                }

                var vendaCriada = JsonConvert.DeserializeObject<Venda>(await response.Content.ReadAsStringAsync());

                // 2. Enviar os itens do carrinho para a API de VendaItens
                var meusItens = HttpContext.Session.GetObjectFromJson<List<CarrinhoItem>>("Carrinho");

                if (meusItens == null || !meusItens.Any())
                {
                    ViewData["ErrorMessage"] = "O carrinho está vazio.";
                    return View("FinalizarCompra", venda);
                }

                var vendaItens = meusItens.Select(item => new VendaItens
                {
                    IdVenda = vendaCriada.Id,
                    IdProduto = item.ProdutoId,
                    Quantidade = item.Quantidade,
                    Preco = item.PrecoUnitario,
                }).ToList();

                var vendaItensJson = JsonConvert.SerializeObject(vendaItens);
                var itensResponse = await _httpClient.PostAsync("api/venda-itens/InserirItensVenda",
                    new StringContent(vendaItensJson, Encoding.UTF8, "application/json"));

                if (!itensResponse.IsSuccessStatusCode)
                {
                    ViewData["ErrorMessage"] = "Erro ao adicionar os itens da venda.";
                    return View("FinalizarCompra", venda);
                }

                // Limpa o carrinho da sessão
                HttpContext.Session.Remove("Carrinho");

                return RedirectToAction("Sucesso");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Erro: {ex.Message}";
                return View("FinalizarCompra", venda);
            }
        }

        // Outros métodos mantêm a lógica original, como Create, Edit, Delete, etc.
    }
}
