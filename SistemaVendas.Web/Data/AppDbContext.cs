using Microsoft.EntityFrameworkCore;
using SistemaVendas.Web.Models;


namespace SistemaVendas.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options){}

    public DbSet<Venda> Vendas { get; set; }
    public DbSet<RelatorioSolicitacao> Relatorios { get; set; }

}