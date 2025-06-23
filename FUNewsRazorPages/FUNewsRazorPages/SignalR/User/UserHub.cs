using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace FUNewsRazorPages.SignalR.User
{
    public class UserHub : Hub
    {
        public static readonly ConcurrentDictionary<string, string> OnlineUsers = new();

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var email = httpContext?.Request.Query["email"].ToString();

            if (!string.IsNullOrEmpty(email))
            {
                OnlineUsers[Context.ConnectionId] = email;
                await Clients.All.SendAsync("UserStatusChanged", email, true); // ✅ thêm await
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (OnlineUsers.TryRemove(Context.ConnectionId, out var email))
            {
                await Clients.All.SendAsync("UserStatusChanged", email, false); // ✅ thêm await
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
