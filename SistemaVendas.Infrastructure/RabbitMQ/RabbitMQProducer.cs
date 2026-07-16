using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using SistemaVendas.Infrastructure.Messages;


namespace SistemaVendas.Infrastructure.RabbitMQ;


public class RabbitMQProducer
{

    private readonly RabbitMQSettings _settings;
    private readonly IRabbitMQConnectionManager _manager;

    public RabbitMQProducer(
     RabbitMQSettings settings,
     IRabbitMQConnectionManager manager)
    {
        _settings = settings;
        _manager = manager;
    }


    public async Task PublicarAsync(
        GerarRelatorioMessage message)
    {
        await using var channel =
            await _manager.CreateChannelAsync();

        await channel.QueueDeclareAsync(

            queue: _settings.QueueRelatorios,

            durable: true,

            exclusive: false,

            autoDelete: false

        );



        var json =
            JsonSerializer.Serialize(message);



        var body =
            Encoding.UTF8.GetBytes(json);



        await channel.BasicPublishAsync(

            exchange: string.Empty,

            routingKey: _settings.QueueRelatorios,

            body: body

        );

    }

}