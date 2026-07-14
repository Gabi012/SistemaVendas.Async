using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SistemaVendas.Infrastructure.Data;
using SistemaVendas.Infrastructure.RabbitMQ;
using SistemaVendas.Worker;
using SistemaVendas.Worker.Consumers;
using SistemaVendas.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<AppDbContext>(
    options =>
    {
        options.UseSqlServer(
            builder.Configuration
            .GetConnectionString("DefaultConnection"));
    });


builder.Services.Configure<RabbitMQSettings>(
    builder.Configuration.GetSection("RabbitMQ"));


builder.Services.AddSingleton(sp =>
{
    return sp.GetRequiredService<
        IOptions<RabbitMQSettings>>()
        .Value;
});


builder.Services.AddSingleton<RelatorioConsumer>();
builder.Services.AddScoped<RelatorioService>();
builder.Services.AddScoped<GeradorRelatorioService>();
builder.Services.AddScoped<EmailService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
