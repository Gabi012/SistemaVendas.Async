using Microsoft.AspNetCore.SignalR;


namespace SistemaVendas.Web.Hubs;


public class NotificacaoHub : Hub
{

    public override async Task OnConnectedAsync()
    {

        var usuarioId =
            Context.GetHttpContext()
            ?.Request
            .Query["usuarioId"]
            .ToString();


        if (!string.IsNullOrEmpty(usuarioId))
        {

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                usuarioId);

        }


        await base.OnConnectedAsync();

    }

}