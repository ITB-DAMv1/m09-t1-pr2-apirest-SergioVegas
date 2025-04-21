using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace API_GAMES.Hubs
{
    [Authorize]
    public class Xat : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Envia el missatge a tots els clients connectats
            await Clients.All.SendAsync("RepMissatge", user, message);
        }

        //Event per control de qui es connecta
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"Nou client: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        //Event per control de qui es desconnecta
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            Console.WriteLine($"Client desconnectat: {Context.ConnectionId}");
            await base.OnDisconnectedAsync(ex);
        }
    }
}
