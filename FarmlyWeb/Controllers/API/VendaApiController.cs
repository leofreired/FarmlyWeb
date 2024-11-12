using FarmlyWeb.Models;
using FarmlyWeb.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FarmlyWeb.Controllers.Api
{
    [Route("api/venda")]
    [ApiController]
    public class VendaApiController : ControllerBase
    {
        private readonly Contexto _context;

        public VendaApiController(Contexto context)
        {
            _context = context;
        }

        // POST: api/vendas
        [HttpPost]
        public async Task<IActionResult> CriarVenda([FromBody] VendaDTO vendaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Cria uma nova instância da Venda com os dados do DTO
            var venda = new Venda
            {
                IdCliente = vendaDto.IdCliente,
                DataVenda = vendaDto.DataVenda,
                Preco = vendaDto.PrecoTotal,
                Pagamento = vendaDto.Pagamento,
                Status = vendaDto.Status
            };

            // Adiciona e salva a nova venda no banco de dados
            _context.Venda.Add(venda);
            await _context.SaveChangesAsync();

            // Retorna a venda criada com o código 201 (Created)
            return CreatedAtAction(nameof(CriarVenda), new { id = venda.Id }, venda);
        }
    }
}
