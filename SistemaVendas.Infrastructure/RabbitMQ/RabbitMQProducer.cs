using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using SistemaVendas.Infrastructure.Messages;


namespace SistemaVendas.Infrastructure.RabbitMQ;


public class RabbitMQProducer
{

    private readonly RabbitMQSettings _settings;


    public RabbitMQProducer(
        RabbitMQSettings settings)
    {
        _settings = settings;
    }



    public async Task PublicarAsync(
        GerarRelatorioMessage message)
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



        var json =
            JsonSerializer.Serialize(message);



        var body =
            Encoding.UTF8.GetBytes(json);



        await channel.BasicPublishAsync(

            exchange: string.Empty,

            routingKey: _settings.QueueName,

            body: body

        );

    }

}