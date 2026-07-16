using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaVendas.Worker.Services
{
    public class GeradorRelatorioService
    {
        public async Task<string> GerarAsync(Guid relatorioId,string tipoRelatorio)
        {
            Console.WriteLine($"Iniciando geração: {tipoRelatorio}");

            // Simula consulta pesada no banco
            await Task.Delay(5000);

            // Simula geração do arquivo
            await Task.Delay(5000);

            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "Relatorios");

            if (!Directory.Exists(pasta)) 
            {
                Directory.CreateDirectory(pasta);
            }

            var nomeArquivo = $"relatorio_{relatorioId}.csv";

            var caminho = Path.Combine(pasta, nomeArquivo);
            // Conteúdo fictício (não é um PDF válido)
            var conteudo = $"""
            Relatório: {tipoRelatorio}

            Id: {relatorioId}

            Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm:ss}
            """;
            await File.WriteAllTextAsync(caminho, conteudo);

            Console.WriteLine(
            $"Arquivo criado: {caminho}");

            return caminho;

        }
    }
}
