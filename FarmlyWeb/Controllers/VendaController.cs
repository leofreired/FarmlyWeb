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

namespace FarmlyWeb.Controllers
{
    public class VendaController : Controller
    {
        private readonly Contexto _context;
        private readonly HttpClient _httpClient;

        public VendaController(Contexto context)
        {
            _context = context;
            _httpClient = new HttpClient();
            // Configure a URL base da sua API aqui
            _httpClient.BaseAddress = new Uri("https://localhost:7186/");
        }

        // GET: Venda
        public async Task<IActionResult> Index()
        {
            return View(await _context.Venda.ToListAsync());
        }

        // GET: Venda/FinalizarCompra
        public IActionResult FinalizarCompra(IEnumerable<CarrinhoItem> meusItens)
        {
            var soma = meusItens.Sum(x => x.Total);

            var model = new Venda
            {
                IdCliente = HttpContext.Session.GetInt32("ClientId").GetValueOrDefault(), // Substitua pelo Id do cliente autenticado
                DataVenda = DateTime.Now,
                Preco = soma, // Total será calculado na view
                Pagamento = string.Empty,
                Status = "0" // Status inicial como Pendente
            };

            return View(model);
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

                var vendaDto = new VendaDTO
                {
                    IdCliente = venda.IdCliente,
                    DataVenda = venda.DataVenda,
                    PrecoTotal = venda.Preco,
                    Pagamento = venda.Pagamento,
                    Status = venda.Status
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(vendaDto), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/venda", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Produto");
                }
                else
                {
                    ViewData["ErrorMessage"] = "Ocorreu um erro ao finalizar a compra. Tente novamente.";
                    return View("FinalizarCompra", venda);
                }
            }
            catch (Exception)
            {
                return View("FinalizarCompra", venda);
            }
        }


        // GET: Venda/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // GET: Venda/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venda/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCliente,DataVenda,Preco,Pagamento,Status")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venda);
        }

        // GET: Venda/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda.FindAsync(id);
            if (venda == null)
            {
                return NotFound();
            }
            return View(venda);
        }

        // POST: Venda/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCliente,DataVenda,Preco,Pagamento,Status")] Venda venda)
        {
            if (id != venda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(venda);
        }

        // GET: Venda/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Venda
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Venda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Venda.FindAsync(id);
            if (venda != null)
            {
                _context.Venda.Remove(venda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Venda.Any(e => e.Id == id);
        }
    }
}
