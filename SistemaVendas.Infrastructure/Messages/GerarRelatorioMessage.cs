namespace SistemaVendas.Infrastructure.Messages;


public class GerarRelatorioMessage
{
    public Guid RelatorioId { get; set; }
    public string TipoRelatorio { get; set; } = string.Empty;
    public string EmailUsuario { get; set; } = string.Empty;
    public Guid UsuarioId { get; set; }


}