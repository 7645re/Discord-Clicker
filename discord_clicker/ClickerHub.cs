using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace discord_clicker;

public class ClickerHub : Hub
{
    public async Task Send(string message)
    {
        await this.Clients.All.SendAsync("Send", message);
    }
}