using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SistemaVendas.Infrastructure.Messages;
using SistemaVendas.Infrastructure.RabbitMQ;
using SistemaVendas.Worker.Services;


namespace SistemaVendas.Worker.Consumers;


public class RelatorioConsumer
{

    private readonly RabbitMQSettings _settings;

    private readonly IServiceScopeFactory _scopeFactory;



    public RelatorioConsumer(
        RabbitMQSettings settings,
        IServiceScopeFactory scopeFactory)
    {
        _settings = settings;

        _scopeFactory = scopeFactory;
    }




    public async Task ConsumirAsync(
        CancellationToken cancellationToken)
    {


        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,

            Port = _settings.Port,

            UserName = _settings.UserName,

            Password = _settings.Password
        };



        await using var connection =
            await factory.CreateConnectionAsync();



        await using var channel =
            await connection.CreateChannelAsync();



        await channel.QueueDeclareAsync(
            queue: _settings.QueueName,

            durable: true,

            exclusive: false,

            autoDelete: false
        );




        var consumer =
            new AsyncEventingBasicConsumer(channel);



        consumer.ReceivedAsync += async (sender, args) =>
        {

            try
            {

                var body =
                    args.Body.ToArray();



                var json =
                    Encoding.UTF8.GetString(body);



                var mensagem =
                    JsonSerializer.Deserialize
                    <GerarRelatorioMessage>(json);



                if (mensagem == null)
                    return;



                Console.WriteLine(
                    $"Recebido: {mensagem.TipoRelatorio}");



                await Processar(mensagem);



                await channel.BasicAckAsync(
                    args.DeliveryTag,
                    false);

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

            queue: _settings.QueueName,

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







    private async Task Processar(
        GerarRelatorioMessage mensagem)
    {


        using var scope =
            _scopeFactory.CreateScope();



        var service =
            scope.ServiceProvider
            .GetRequiredService<RelatorioService>();



        await service.AtualizarStatusAsync(
            mensagem.RelatorioId,
            "Processando");



        Console.WriteLine(
            "Processando relatório...");



        await Task.Delay(
            TimeSpan.FromSeconds(10));



        await service.AtualizarStatusAsync(
            mensagem.RelatorioId,
            "Concluído");


        Console.WriteLine(
            "Relatório concluído");

    }

}