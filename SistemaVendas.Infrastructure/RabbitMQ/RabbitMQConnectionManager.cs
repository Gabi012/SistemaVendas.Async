using RabbitMQ.Client;

namespace SistemaVendas.Infrastructure.RabbitMQ;

public class RabbitMQConnectionManager : IRabbitMQConnectionManager, IAsyncDisposable
{
    private readonly RabbitMQSettings _settings;

    private IConnection? _connection;

    private readonly SemaphoreSlim _lock = new(1, 1);

    public RabbitMQConnectionManager(RabbitMQSettings settings)
    {
        _settings = settings;
    }

    public async Task<IConnection> GetConnectionAsync()
    {
        if (_connection is { IsOpen: true })
            return _connection;

        await _lock.WaitAsync();

        try
        {
            if (_connection is { IsOpen: true })
                return _connection;

            var factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };

            _connection = await factory.CreateConnectionAsync();

            Console.WriteLine("RabbitMQ Connection criada.");

            return _connection;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IChannel> CreateChannelAsync()
    {
        var connection = await GetConnectionAsync();

        return await connection.CreateChannelAsync();
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
            await _connection.DisposeAsync();

        _lock.Dispose();
    }
}