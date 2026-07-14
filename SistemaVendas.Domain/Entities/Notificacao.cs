namespace SistemaVendas.Domain.Entities;


public class Notificacao
{

    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public Guid? RelatorioId { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public bool Lida { get; set; }


}