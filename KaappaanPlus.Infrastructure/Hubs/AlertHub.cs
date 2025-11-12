using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace KaappaanPlus.WebApi.Hubs
{
    public class AlertHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"🔗 Connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        public async Task JoinResponderGroup(string role)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, role);
            Console.WriteLine($"✅ {Context.ConnectionId} joined {role}");
        }
    }
}
