using SistemaVendas.Infrastructure.RabbitMQ;
using SistemaVendas.Worker;
using SistemaVendas.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<RabbitMQSettings>();
builder.Services.AddSingleton<RelatorioConsumer>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
