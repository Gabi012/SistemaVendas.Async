namespace SistemaVendas.Domain.Entities;

public class RelatorioSolicitacao
{

    public Guid Id { get; set; }


    public string TipoRelatorio { get; set; } = string.Empty;


    public DateTime DataSolicitacao { get; set; }


    public DateTime? DataConclusao { get; set; }


    public string Status { get; set; } = string.Empty;


    public string ArquivoGerado { get; set; } = string.Empty;

}