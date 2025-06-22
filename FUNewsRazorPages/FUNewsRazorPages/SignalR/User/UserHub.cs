using Microsoft.AspNetCore.SignalR;

namespace FUNewsRazorPages.SignalR.User
{
    public class UserHub : Hub
    {
        public static readonly Dictionary<string, string> OnlineUsers = new();

        public override Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var email = httpContext?.Session.GetString("AccountEmail");

            if (!string.IsNullOrEmpty(email))
            {
                OnlineUsers[Context.ConnectionId] = email;
                Clients.All.SendAsync("UserStatusChanged", email, true);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            if (OnlineUsers.TryGetValue(Context.ConnectionId, out var email))
            {
                OnlineUsers.Remove(Context.ConnectionId);
                Clients.All.SendAsync("UserStatusChanged", email, false);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
