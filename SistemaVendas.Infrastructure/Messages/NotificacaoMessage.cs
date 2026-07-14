namespace SistemaVendas.Infrastructure.Messages;


public class NotificacaoMessage
{
    public Guid UsuarioId { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public Guid RelatorioId { get; set; }


}