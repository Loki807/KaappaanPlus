using Microsoft.AspNetCore.SignalR;

namespace KaappaanPlus.WebApi.Hubs
{
    public class AlertHub : Hub
    {
        // 🌍 Static tracker for proximity filtering (Responder ConnectionId -> Lat/Lng)
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, (double Lat, double Lng)> _responderLocations 
            = new System.Collections.Concurrent.ConcurrentDictionary<string, (double, double)>();

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _responderLocations.TryRemove(Context.ConnectionId, out _);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task UpdateIdleLocation(double lat, double lng)
        {
            _responderLocations[Context.ConnectionId] = (lat, lng);
        }

        // Helper for backend to get connection IDs near a point
        public static IEnumerable<string> GetConnectionsNear(double lat, double lng, double radiusKm)
        {
            return _responderLocations
                .Where(x => CalculateDistance(lat, lng, x.Value.Lat, x.Value.Lng) <= radiusKm)
                .Select(x => x.Key);
        }

        private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var R = 6371; // Radius of the earth in km
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRadians(double deg) => deg * (Math.PI / 180);

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
