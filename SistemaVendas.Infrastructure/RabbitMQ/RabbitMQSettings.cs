namespace SistemaVendas.Infrastructure.RabbitMQ;


public class RabbitMQSettings
{

    public string HostName { get; set; }
        = "127.0.0.1";


    public int Port { get; set; }
        = 5672;


    public string UserName { get; set; }
        = "guest";


    public string Password { get; set; }
        = "guest";


    public string QueueName { get; set; }
        = "relatorios";

}