using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmlyWeb.Models;
using Newtonsoft.Json;
using FarmlyWeb.DTOs;

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
        public IActionResult FinalizarCompra()
        {
            var model = new VendaViewModel
            {
                IdCliente = 1, // Substitua pelo Id do cliente autenticado
                DataVenda = DateTime.Now,
                PrecoTotal = 0, // Total será calculado na view
                Pagamento = string.Empty,
                Status = 1 // Status inicial como Pendente
            };

            return View(model);
        }

        // POST: Venda/FinalizarCompra
        [HttpPost]
        public async Task<IActionResult> FinalizarCompra(VendaViewModel vendaViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(vendaViewModel);
            }

            // Monta o objeto DTO para enviar à API
            var vendaDto = new VendaDTO
            {
                IdCliente = vendaViewModel.IdCliente,
                DataVenda = vendaViewModel.DataVenda,
                PrecoTotal = vendaViewModel.PrecoTotal,
                Pagamento = vendaViewModel.Pagamento,
                Status = vendaViewModel.Status
            };

            // Serializa o objeto para JSON
            var jsonContent = new StringContent(JsonConvert.SerializeObject(vendaDto), Encoding.UTF8, "application/json");

            // Faz a chamada POST para a API de vendas
            var response = await _httpClient.PostAsync("api/venda", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Carrinho"); // Redireciona após a compra ser finalizada com sucesso
            }
            else
            {
                // Exibe erro se houver problema ao finalizar a compra
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao finalizar a compra. Tente novamente.");
                return View(vendaViewModel);
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
