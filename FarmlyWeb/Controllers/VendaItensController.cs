using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FarmlyWeb.Models;

namespace FarmlyWeb.Controllers
{
    public class VendaItensController : BaseController
    {
        private readonly Contexto _context;

        public VendaItensController(Contexto context)
        {
            _context = context;
        }

        // GET: VendaItens
        public async Task<IActionResult> Index()
        {
            return View(await _context.VendaItens.ToListAsync());
        }

        // GET: VendaItens/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendaItens = await _context.VendaItens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendaItens == null)
            {
                return NotFound();
            }

            return View(vendaItens);
        }

        // GET: VendaItens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VendaItens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdVenda,IdProduto,Quantidade,Preco")] VendaItens vendaItens)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendaItens);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vendaItens);
        }

        // GET: VendaItens/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendaItens = await _context.VendaItens.FindAsync(id);
            if (vendaItens == null)
            {
                return NotFound();
            }
            return View(vendaItens);
        }

        // POST: VendaItens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdVenda,IdProduto,Quantidade,Preco")] VendaItens vendaItens)
        {
            if (id != vendaItens.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendaItens);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaItensExists(vendaItens.Id))
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
            return View(vendaItens);
        }

        // GET: VendaItens/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vendaItens = await _context.VendaItens
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vendaItens == null)
            {
                return NotFound();
            }

            return View(vendaItens);
        }

        // POST: VendaItens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vendaItens = await _context.VendaItens.FindAsync(id);
            if (vendaItens != null)
            {
                _context.VendaItens.Remove(vendaItens);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaItensExists(int id)
        {
            return _context.VendaItens.Any(e => e.Id == id);
        }
    }
}
