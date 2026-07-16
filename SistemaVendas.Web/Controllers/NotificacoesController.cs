using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVendas.Infrastructure.Data;
using SistemaVendas.Web.Models;


namespace SistemaVendas.Web.Controllers;


public class NotificacoesController : Controller
{

    private readonly AppDbContext _context;


    public NotificacoesController(
        AppDbContext context)
    {
        _context = context;
    }



    public async Task<IActionResult> Index()
    {

        // temporário até termos login
        var usuarioId =
            Guid.Parse("11111111-1111-1111-1111-111111111111");


        var notificacoes = await _context.Notificacoes
            .Where(x => x.UsuarioId == usuarioId)
            .OrderByDescending(x => x.DataCriacao)
            .Select(x => new NotificacaoViewModel
            {
                Id = x.Id,
                Mensagem = x.Mensagem,
                DataCriacao = x.DataCriacao,
                Lida = x.Lida,
                RelatorioId = x.RelatorioId,
                ArquivoGerado = _context.Relatorios
                    .Where(r => r.Id == x.RelatorioId)
                    .Select(r => r.ArquivoGerado)
                    .FirstOrDefault()
            })
            .ToListAsync();


        return View(notificacoes);

    }

    [HttpGet]
    public async Task<IActionResult> Contador()
    {

        var usuarioId =
            Guid.Parse(
            "11111111-1111-1111-1111-111111111111");


        var total = await _context.Notificacoes
                                    .CountAsync(x =>
                                    x.UsuarioId == usuarioId &&
                                    !x.Lida);


        return Json(total);

    }
    [HttpPost]
    public async Task<IActionResult> MarcarComoLida(Guid id)
    {
        var notificacao =
            await _context.Notificacoes
                .FirstOrDefaultAsync(x => x.Id == id);

        if (notificacao == null)
            return NotFound();

        notificacao.Lida = true;

        await _context.SaveChangesAsync();

        return Ok();
    }
  
}