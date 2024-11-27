using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FarmlyWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmlyWeb.Controllers.Api
{
    [Route("api/venda-itens")]
    [ApiController]
    public class VendaItensApiController : ControllerBase
    {
        private readonly Contexto _context;

        public VendaItensApiController(Contexto context)
        {
            _context = context;
        }

        // POST: api/venda-itens/InserirItensVenda
        [HttpPost("InserirItensVenda")]
        public async Task<IActionResult> InserirItensVenda([FromBody] List<VendaItens> vendaItens)
        {
            if (vendaItens == null || !vendaItens.Any())
            {
                return BadRequest("Nenhum item foi enviado.");
            }

            try
            {
                // Adiciona todos os itens enviados na tabela VendaItens
                _context.VendaItens.AddRange(vendaItens);
                await _context.SaveChangesAsync();

                return Ok(new { mensagem = "Itens inseridos com sucesso!", itens = vendaItens });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao inserir os itens: {ex.Message}");
            }
        }
    }
}
