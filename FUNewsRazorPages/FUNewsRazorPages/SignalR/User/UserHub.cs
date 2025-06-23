using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace FUNewsRazorPages.SignalR.User
{
    public class UserHub : Hub
    {
        // Use ConcurrentDictionary for thread-safe operations
        public static readonly ConcurrentDictionary<string, string> ConnectionUsers = new();
        
        // Map: Email -> List of ConnectionIds (support multiple tabs/browsers)
        public static readonly ConcurrentDictionary<string, HashSet<string>> UserConnections = new();
        private static readonly object _lock = new object();

        public override async Task OnConnectedAsync()
        {
            // Get email from query string instead of session
            var httpContext = Context.GetHttpContext();
            var email = httpContext?.Request.Query["email"].ToString();
            var clientIp = httpContext?.Connection?.RemoteIpAddress?.ToString();
            var userAgent = httpContext?.Request.Headers["User-Agent"].ToString();
            var referer = httpContext?.Request.Headers["Referer"].ToString();

            Console.WriteLine($"=== UserHub: Connection attempt ===");
            Console.WriteLine($"ConnectionId: {Context.ConnectionId}");
            Console.WriteLine($"Email from query: '{email}'");
            Console.WriteLine($"Client IP: {clientIp}");
            Console.WriteLine($"Referer: {referer}");
            Console.WriteLine($"User Agent: {userAgent?.Substring(0, Math.Min(userAgent.Length, 100))}...");
            Console.WriteLine($"Host: {httpContext?.Request.Host}");
            Console.WriteLine($"Current connections before: {ConnectionUsers.Count}");

            if (!string.IsNullOrEmpty(email))
            {
                lock (_lock)
                {
                    // Add connection mapping
                    ConnectionUsers[Context.ConnectionId] = email;
                    
                    // Add to user connections set
                    if (!UserConnections.ContainsKey(email))
                    {
                        UserConnections[email] = new HashSet<string>();
                    }
                    UserConnections[email].Add(Context.ConnectionId);
                }
                
                Console.WriteLine($"✅ User {email} connected from {clientIp}. Total connections for user: {UserConnections[email].Count}");
                Console.WriteLine($"✅ Total system connections: {ConnectionUsers.Count}");
                Console.WriteLine($"✅ Total unique users online: {UserConnections.Count}");
                
                // Notify all clients that user is online (outside lock)
                await Clients.All.SendAsync("UserStatusChanged", email, true);
            }
            else
            {
                Console.WriteLine($"❌ Connection failed: No email provided from {clientIp}");
                Console.WriteLine($"❌ Query string: {httpContext?.Request.QueryString}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var clientIp = httpContext?.Connection?.RemoteIpAddress?.ToString();
            
            Console.WriteLine($"=== UserHub: Disconnection ===");
            Console.WriteLine($"ConnectionId: {Context.ConnectionId}");
            Console.WriteLine($"Client IP: {clientIp}");
            Console.WriteLine($"Exception: {exception?.Message}");
            Console.WriteLine($"Current connections before removal: {ConnectionUsers.Count}");

            bool shouldNotifyOffline = false;
            string? emailToNotify = null;

            if (ConnectionUsers.TryRemove(Context.ConnectionId, out var email))
            {
                lock (_lock)
                {
                    // Remove from user connections set
                    if (UserConnections.ContainsKey(email))
                    {
                        UserConnections[email].Remove(Context.ConnectionId);
                        
                        Console.WriteLine($"User {email} disconnected from {clientIp}. Remaining connections for user: {UserConnections[email].Count}");
                        
                        // If user has no more connections, mark as offline
                        if (UserConnections[email].Count == 0)
                        {
                            UserConnections.TryRemove(email, out _);
                            Console.WriteLine($"🔴 User {email} is now offline");
                            shouldNotifyOffline = true;
                            emailToNotify = email;
                        }
                    }
                }

                Console.WriteLine($"✅ Total system connections after removal: {ConnectionUsers.Count}");
                Console.WriteLine($"✅ Total unique users online after removal: {UserConnections.Count}");

                // Notify outside lock to avoid async in lock
                if (shouldNotifyOffline && !string.IsNullOrEmpty(emailToNotify))
                {
                    await Clients.All.SendAsync("UserStatusChanged", emailToNotify, false);
                }
            }
            else
            {
                Console.WriteLine($"❌ Disconnection failed: ConnectionId not found for {clientIp}");
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

        // Debug method to get connection info
        public static Dictionary<string, object> GetConnectionInfo()
        {
            Console.WriteLine($"=== UserHub Connection Info ===");
            Console.WriteLine($"Total Connections: {ConnectionUsers.Count}");
            Console.WriteLine($"Unique Users: {UserConnections.Count}");
            Console.WriteLine($"Online Users: {string.Join(", ", UserConnections.Keys)}");
            
            return new Dictionary<string, object>
            {
                {"TotalConnections", ConnectionUsers.Count},
                {"UniqueUsers", UserConnections.Count},
                {"OnlineUsers", UserConnections.Keys.ToList()},
                {"ConnectionDetails", UserConnections.ToDictionary(
                    kvp => kvp.Key, 
                    kvp => kvp.Value.Count
                )}
            };
        }

        // Method to manually trigger status update (for debugging)
        public async Task RefreshUserStatus(string email)
        {
            bool isOnline = IsUserOnline(email);
            Console.WriteLine($"Manual refresh for {email}: {(isOnline ? "Online" : "Offline")}");
            await Clients.All.SendAsync("UserStatusChanged", email, isOnline);
        }
    }
}
