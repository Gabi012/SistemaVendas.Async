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
           EmailUsuario = "usuario@email.com"
       }

   );

        return RedirectToAction(nameof(Index));

    }

}