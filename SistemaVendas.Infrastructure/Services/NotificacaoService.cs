using Microsoft.EntityFrameworkCore;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Data;


namespace SistemaVendas.Infrastructure.Services;


public class NotificacaoService
{

    private readonly AppDbContext _context;


    public NotificacaoService(
        AppDbContext context)
    {
        _context = context;
    }



    public async Task CriarAsync(Guid usuarioId,Guid relatorioId,string mensagem)
    {

        var notificacao =
            new Notificacao
            {
                Id = Guid.NewGuid(),

                UsuarioId = usuarioId,

                RelatorioId = relatorioId,

                Mensagem = mensagem,

                DataCriacao = DateTime.Now,

                Lida = false

            };


        await _context.Notificacoes
            .AddAsync(notificacao);


        await _context.SaveChangesAsync();

    }



    public async Task<List<Notificacao>>BuscarNaoLidasAsync(Guid usuarioId)
    {
        return await _context.Notificacoes.Where(x =>x.UsuarioId == usuarioId &&!x.Lida).OrderByDescending(x => x.DataCriacao).ToListAsync();

    }


}