using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVendas.Infrastructure.Messages;
using SistemaVendas.Infrastructure.RabbitMQ;
using SistemaVendas.Infrastructure.Data;
using SistemaVendas.Domain.Entities;


namespace SistemaVendas.Web.Controllers;


public class RelatoriosController : Controller
{
    private readonly RabbitMQProducer _rabbit;
    private readonly AppDbContext _context;



    public RelatoriosController(
      AppDbContext context,
      RabbitMQProducer rabbit)
    {
        _context = context;

        _rabbit = rabbit;
    }





    public async Task<IActionResult> Index()
    {

        var relatorios = await _context.Relatorios
            .OrderByDescending(x => x.DataSolicitacao)
            .ToListAsync();


        return View(relatorios);

    }


    public IActionResult Solicitar()  
    {
        return View();
    }



    [HttpPost]

    public async Task<IActionResult> Solicitar(string tipoRelatorio)
    {

        var relatorio = new RelatorioSolicitacao
        {

            Id = Guid.NewGuid(),

            TipoRelatorio = tipoRelatorio,

            DataSolicitacao = DateTime.Now,

            Status = "Pendente"

        };

        _context.Relatorios.Add(relatorio);

        await _context.SaveChangesAsync();

        await _rabbit.PublicarAsync(new GerarRelatorioMessage
       {
           RelatorioId = relatorio.Id,
           TipoRelatorio = relatorio.TipoRelatorio,
           EmailUsuario = "usuario@email.com",
           UsuarioId = Guid.Parse("11111111-1111-1111-1111-111111111111")
        }

   );

        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Download(Guid id)
    {
        var relatorio =
            await _context.Relatorios
                .FirstOrDefaultAsync(x => x.Id == id);

        if (relatorio == null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(relatorio.ArquivoGerado))
            return BadRequest("Relatório ainda não foi gerado.");

        if (!System.IO.File.Exists(relatorio.ArquivoGerado))
            return NotFound("Arquivo não encontrado.");

        var bytes =
            await System.IO.File.ReadAllBytesAsync(relatorio.ArquivoGerado);

        return File(
            bytes,
            "application/pdf",
            Path.GetFileName(relatorio.ArquivoGerado));
    }

}