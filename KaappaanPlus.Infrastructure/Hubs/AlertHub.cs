using Microsoft.AspNetCore.SignalR;

namespace KaappaanPlus.WebApi.Hubs
{
    public class AlertHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine($"🔗 Connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        // Citizen or Responder joins the alert group
        public async Task JoinAlertGroup(string alertId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, alertId);
            Console.WriteLine($"✔ Joined alert group: {alertId} ({Context.ConnectionId})");
        }

        // Optional: for leaving
        public async Task LeaveAlertGroup(string alertId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, alertId);
            Console.WriteLine($"❌ Left alert group: {alertId} ({Context.ConnectionId})");
        }

        public async Task JoinRoleGroup(string role)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, role);
            Console.WriteLine($"Responder joined role group: {role}");
        }

    }
}
