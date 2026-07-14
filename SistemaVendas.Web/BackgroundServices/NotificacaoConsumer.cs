using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SistemaVendas.Infrastructure.Messages;
using SistemaVendas.Infrastructure.RabbitMQ;
using SistemaVendas.Web.Hubs;


namespace SistemaVendas.Web.BackgroundServices;


public class NotificacaoConsumer : BackgroundService
{

    private readonly RabbitMQSettings _settings;

    private readonly IHubContext<NotificacaoHub> _hub;

    public NotificacaoConsumer(RabbitMQSettings settings,IHubContext<NotificacaoHub> hub)
    {
        _settings = settings;
        _hub = hub;
    }




    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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

            queue: _settings.QueueNotificacoes,

            durable: true,

            exclusive: false,

            autoDelete: false

        );




        var consumer =
            new AsyncEventingBasicConsumer(channel);



        consumer.ReceivedAsync += async (sender, args) =>
        {

            var body = args.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);

            var mensagem = JsonSerializer.Deserialize<NotificacaoMessage>(json);



            if (mensagem != null)
            {
                Console.WriteLine(mensagem.Mensagem);

                await _hub.Clients
                    .Group(mensagem.UsuarioId.ToString())
                    .SendAsync(
                        "ReceberNotificacao",
                        mensagem.Mensagem);

            }



            await channel.BasicAckAsync(
                args.DeliveryTag,
                false);

        };




        await channel.BasicConsumeAsync(

            queue: _settings.QueueNotificacoes,

            autoAck: false,

            consumer: consumer

        );



        Console.WriteLine("Consumidor de notificações iniciado");



        while (!stoppingToken.IsCancellationRequested)
        {

            await Task.Delay(1000);

        }

    }

}