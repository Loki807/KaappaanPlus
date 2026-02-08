using Microsoft.AspNetCore.SignalR;

namespace KaappaanPlus.WebApi.Hubs
{
    public class AlertHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            // Console.WriteLine($"🔗 Connected: {Context.ConnectionId}");
            await base.OnConnectedAsync();
        }

        // Citizen or Responder joins the alert group
        public async Task JoinAlertGroup(string alertId)
        {
            if (string.IsNullOrEmpty(alertId)) return;

            var userId = Context.User?.Identity?.Name ?? "Anonymous";
            var normalizedId = alertId.ToLower();
            
            await Groups.AddToGroupAsync(Context.ConnectionId, normalizedId);
            
            // Console.WriteLine($"✔ Join: User {userId} joined group {normalizedId} (Conn: {Context.ConnectionId})");
            
            // Send confirmation back to the caller
            await Clients.Caller.SendAsync("AlertGroupJoined", normalizedId);
        }

        // Optional: for leaving
        public async Task LeaveAlertGroup(string alertId)
        {
            if (string.IsNullOrEmpty(alertId)) return;

            var userId = Context.User?.Identity?.Name ?? "Anonymous";
            var normalizedId = alertId.ToLower();
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, normalizedId);
            // Console.WriteLine($"❌ Leave: User {userId} left group {normalizedId} (Conn: {Context.ConnectionId})");
        }

        public async Task JoinRoleGroup(string role)
        {
            if (string.IsNullOrEmpty(role)) return;

            await Groups.AddToGroupAsync(Context.ConnectionId, role);
            // Console.WriteLine($"Responder joined role group: {role}");
        }

        public async Task StreamLocation(string alertId, double lat, double lng, float heading, float speed)
        {
            if (string.IsNullOrEmpty(alertId)) return;
            var normalizedId = alertId.ToLower();

            // Broadcast to everyone in the Alert Group (e.g. Citizen)
            await Clients.Group(normalizedId).SendAsync("ResponderLocationUpdated", new 
            {
                AlertId = normalizedId,
                Latitude = lat,
                Longitude = lng,
                Heading = heading,
                Speed = speed
            });
            // Optimization: Logging removed for high-frequency location streaming
        }

      


    }
}
