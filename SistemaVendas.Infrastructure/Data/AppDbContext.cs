using Microsoft.EntityFrameworkCore;
using SistemaVendas.Domain.Entities;


namespace SistemaVendas.Infrastructure.Data;


public class AppDbContext : DbContext
{

    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {

    }

    public DbSet<RelatorioSolicitacao> Relatorios { get; set; }

    public DbSet<Venda> Vendas { get; set; }
    public DbSet<Notificacao> Notificacoes { get; set; }


}