
using RabbitMQ.Client;

namespace SistemaVendas.Infrastructure.RabbitMQ
{
    public interface IRabbitMQConnectionManager
    {
        Task<IConnection> GetConnectionAsync();
        Task<IChannel> CreateChannelAsync();
    }
}
