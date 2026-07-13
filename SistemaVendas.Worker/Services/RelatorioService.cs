using Microsoft.EntityFrameworkCore;
using SistemaVendas.Infrastructure.Data;


namespace SistemaVendas.Worker.Services;


public class RelatorioService
{

    private readonly AppDbContext _context;



    public RelatorioService(
        AppDbContext context)
    {
        _context = context;
    }





    public async Task AtualizarStatusAsync(
        Guid id,
        string status)
    {


        var relatorio =
            await _context.Relatorios
            .FirstOrDefaultAsync(
                x => x.Id == id);



        if (relatorio == null)
        {

            Console.WriteLine(
                $"Relatório {id} não encontrado");

            return;

        }



        relatorio.Status = status;



        if (status == "Concluído")
        {
            relatorio.DataConclusao =
                DateTime.Now;
        }



        await _context.SaveChangesAsync();



        Console.WriteLine(
            $"Relatório {id} => {status}");

    }

}