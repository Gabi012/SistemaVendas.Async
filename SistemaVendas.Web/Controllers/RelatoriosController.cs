using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaVendas.Web.Data;
using SistemaVendas.Web.Models;


namespace SistemaVendas.Web.Controllers;


public class RelatoriosController : Controller
{

    private readonly AppDbContext _context;



    public RelatoriosController(
        AppDbContext context)
    {
        _context = context;
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

        return RedirectToAction(nameof(Index));

    }

}