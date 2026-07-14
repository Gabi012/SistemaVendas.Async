using RabbitMQ.Client;

namespace SistemaVendas.Worker.Services;


public class EmailService
{

    public async Task EnviarAsync(string email,string assunto,string mensagem)
    {

        Console.WriteLine("--------------------------------");
        Console.WriteLine("Enviando e-mail...");
        Console.WriteLine($"Para: {email}");
        Console.WriteLine($"Assunto: {assunto}");
        Console.WriteLine($"Mensagem: {mensagem}");
        Console.WriteLine("--------------------------------");


        // Simula chamada SMTP/API de e-mail
        await Task.Delay(3000);


        Console.WriteLine("E-mail enviado com sucesso");
    }

}