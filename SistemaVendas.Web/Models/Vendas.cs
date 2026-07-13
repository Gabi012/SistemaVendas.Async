namespace SistemaVendas.Web.Models;

public class Venda
{
    public Guid Id { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public DateTime DataVenda { get; set; }
    public string Status { get; set; } = string.Empty;
}