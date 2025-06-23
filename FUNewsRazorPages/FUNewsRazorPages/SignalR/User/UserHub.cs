using Microsoft.AspNetCore.SignalR;

namespace FUNewsRazorPages.SignalR.User
{
    public class UserHub : Hub
    {
        // Map: ConnectionId -> Email
        public static readonly Dictionary<string, string> ConnectionUsers = new();
        
        // Map: Email -> List of ConnectionIds (support multiple tabs/browsers)
        public static readonly Dictionary<string, HashSet<string>> UserConnections = new();

        public override async Task OnConnectedAsync()
        {
            // Get email from query string instead of session
            var httpContext = Context.GetHttpContext();
            var email = httpContext?.Request.Query["email"].ToString();

            Console.WriteLine($"=== UserHub: Connection attempt ===");
            Console.WriteLine($"ConnectionId: {Context.ConnectionId}");
            Console.WriteLine($"Email from query: '{email}'");

            if (!string.IsNullOrEmpty(email))
            {
                // Add connection mapping
                ConnectionUsers[Context.ConnectionId] = email;
                
                // Add to user connections set
                if (!UserConnections.ContainsKey(email))
                {
                    UserConnections[email] = new HashSet<string>();
                }
                UserConnections[email].Add(Context.ConnectionId);
                
                Console.WriteLine($"User {email} connected. Total connections: {UserConnections[email].Count}");
                
                // Notify all clients that user is online
                await Clients.All.SendAsync("UserStatusChanged", email, true);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"=== UserHub: Disconnection ===");
            Console.WriteLine($"ConnectionId: {Context.ConnectionId}");

            if (ConnectionUsers.TryGetValue(Context.ConnectionId, out var email))
            {
                // Remove connection mapping
                ConnectionUsers.Remove(Context.ConnectionId);
                
                // Remove from user connections set
                if (UserConnections.ContainsKey(email))
                {
                    UserConnections[email].Remove(Context.ConnectionId);
                    
                    Console.WriteLine($"User {email} disconnected. Remaining connections: {UserConnections[email].Count}");
                    
                    // If user has no more connections, mark as offline
                    if (UserConnections[email].Count == 0)
                    {
                        UserConnections.Remove(email);
                        Console.WriteLine($"User {email} is now offline");
                        await Clients.All.SendAsync("UserStatusChanged", email, false);
                    }
                }
            }

            await base.OnDisconnectedAsync(exception);
        }
        
        // Helper method to check if user is online
        public static bool IsUserOnline(string email)
        {
            return UserConnections.ContainsKey(email) && UserConnections[email].Count > 0;
        }

        // Get all online users
        public static List<string> GetOnlineUsers()
        {
            return UserConnections.Keys.ToList();
        }
    }
}
