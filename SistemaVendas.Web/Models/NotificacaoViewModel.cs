namespace SistemaVendas.Web.Models;

public class NotificacaoViewModel
{
    public Guid Id { get; set; }

    public string Mensagem { get; set; } = string.Empty;

    public DateTime DataCriacao { get; set; }

    public bool Lida { get; set; }

    public Guid? RelatorioId { get; set; }
}