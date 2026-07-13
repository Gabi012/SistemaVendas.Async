using SistemaVendas.Worker.Consumers;


namespace SistemaVendas.Worker;


public class Worker : BackgroundService
{

    private readonly RelatorioConsumer _consumer;

    public Worker(RelatorioConsumer consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.ConsumirAsync(stoppingToken);

    }

}