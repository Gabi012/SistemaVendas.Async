using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SistemaVendas.Infrastructure.Data;
using SistemaVendas.Infrastructure.Messages;
using SistemaVendas.Infrastructure.RabbitMQ;
using SistemaVendas.Infrastructure.Services;
using SistemaVendas.Worker.Services;


namespace SistemaVendas.Worker.Consumers;


public class RelatorioConsumer
{

    private readonly RabbitMQSettings _settings;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly GeradorRelatorioService _gerador;



    public RelatorioConsumer(RabbitMQSettings settings,IServiceScopeFactory scopeFactory)
    {
        _settings = settings;

        _scopeFactory = scopeFactory;
    }




    public async Task ConsumirAsync(CancellationToken cancellationToken)
    {


        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,

            Port = _settings.Port,

            UserName = _settings.UserName,

            Password = _settings.Password
        };



        await using var connection = await factory.CreateConnectionAsync();

        await using var channel = await connection.CreateChannelAsync();



        await channel.QueueDeclareAsync(queue: _settings.QueueRelatorios,

            durable: true,

            exclusive: false,

            autoDelete: false
        );




        var consumer = new AsyncEventingBasicConsumer(channel);



        consumer.ReceivedAsync += async (sender, args) =>
        {

            try
            {

                var body = args.Body.ToArray();

                var json =Encoding.UTF8.GetString(body);



                var mensagem = JsonSerializer.Deserialize<GerarRelatorioMessage>(json);



                if (mensagem == null)
                    return;


                Console.WriteLine($"Recebido: {mensagem.TipoRelatorio}");


                await Processar(mensagem);



                await channel.BasicAckAsync(args.DeliveryTag,false);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);


                await channel.BasicNackAsync(
                    args.DeliveryTag,
                    false,
                    true);

            }


        };




        await channel.BasicConsumeAsync(

            queue: _settings.QueueRelatorios,

            autoAck: false,

            consumer: consumer

        );



        Console.WriteLine(
            "Consumer iniciado");



        while (!cancellationToken.IsCancellationRequested)
        {

            await Task.Delay(1000);

        }

    }




    private async Task Processar(GerarRelatorioMessage mensagem)
    {

        using var scope =
            _scopeFactory.CreateScope();

        var context =
    scope.ServiceProvider
    .GetRequiredService<AppDbContext>();

        var relatorioService =
            scope.ServiceProvider
            .GetRequiredService<RelatorioService>();


        var gerador =
            scope.ServiceProvider
            .GetRequiredService<GeradorRelatorioService>();

        var notificacao =
           scope.ServiceProvider
           .GetRequiredService<NotificacaoPublisher>();

        var emailService =
            scope.ServiceProvider
            .GetRequiredService<EmailService>();

        var notificacaoService =
       scope.ServiceProvider
       .GetRequiredService<NotificacaoService>();

        var relatorio = await context.Relatorios
                            .FirstOrDefaultAsync(x => x.Id == mensagem.RelatorioId);

        if (relatorio == null)
        {
            Console.WriteLine("Relatório não encontrado.");
            return;
        }
        if (relatorio.Status == "Cancelado")
        {
            Console.WriteLine($"Relatório {relatorio.Id} foi cancelado.");

            return;
        }

        await relatorioService.AtualizarStatusAsync(mensagem.RelatorioId,"Processando");

        try
        {

            var arquivo = await gerador.GerarAsync(mensagem.RelatorioId,mensagem.TipoRelatorio);

            await relatorioService.FinalizarAsync(mensagem.RelatorioId,arquivo);

            var texto = $"Seu relatório {mensagem.TipoRelatorio} está pronto.";

            await notificacaoService.CriarAsync(
                    mensagem.UsuarioId,
                    mensagem.RelatorioId,
                    texto);


            await notificacao.PublicarAsync(
                    new NotificacaoMessage
                    {
                        UsuarioId = mensagem.UsuarioId,
                        RelatorioId = mensagem.RelatorioId,
                        Mensagem = $"Seu relatório {mensagem.TipoRelatorio} está pronto."
                     }

  );

            await emailService.EnviarAsync(
                    mensagem.EmailUsuario,
                    "Relatório disponível",
                    $"Seu relatório {mensagem.TipoRelatorio} foi gerado."

);

        }
        catch (Exception ex)
        {

            Console.WriteLine($"Erro: {ex.Message}");

            await relatorioService.AtualizarStatusAsync(mensagem.RelatorioId,"Erro");

        }

    }

}