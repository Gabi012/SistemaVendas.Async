using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SistemaVendas.Infrastructure.Messages;
using SistemaVendas.Infrastructure.RabbitMQ;


namespace SistemaVendas.Worker.Consumers;


public class RelatorioConsumer
{

    private readonly RabbitMQSettings _settings;


    public RelatorioConsumer(
        RabbitMQSettings settings)
    {
        _settings = settings;
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
            var body = args.Body.ToArray();

            var json =
                Encoding.UTF8.GetString(body);

            var mensagem =
                JsonSerializer.Deserialize
                <GerarRelatorioMessage>(json);


            Console.WriteLine("Mensagem recebida");


            Console.WriteLine($"Relatório: {mensagem?.TipoRelatorio}");


            await Processar(mensagem!);

            await channel.BasicAckAsync(
                args.DeliveryTag,
                false);
        };


        await channel.BasicConsumeAsync(

            queue: _settings.QueueName,
            autoAck: false,
            consumer: consumer
        );

        Console.WriteLine("Consumer iniciado");


        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000);
        }


    }





    private async Task Processar(GerarRelatorioMessage mensagem)
    {


        Console.WriteLine("Iniciando processamento...");


        await Task.Delay(
            TimeSpan.FromSeconds(10));



        Console.WriteLine("Relatório finalizado");

    }


}