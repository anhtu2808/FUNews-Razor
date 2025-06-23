using FUNewsRazorPages.SignalR.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsRazorPages.Pages.Admin
{
    public class SignalRDebugModel : PageModel
    {
        public int TotalConnections { get; set; }
        public int UniqueUsers { get; set; }
        public List<string> OnlineUsers { get; set; } = new();
        public Dictionary<string, object> ConnectionInfo { get; set; } = new();

        public void OnGet()
        {
            // Get connection info from UserHub
            ConnectionInfo = UserHub.GetConnectionInfo();
            
            TotalConnections = (int)ConnectionInfo["TotalConnections"];
            UniqueUsers = (int)ConnectionInfo["UniqueUsers"];
            OnlineUsers = (List<string>)ConnectionInfo["OnlineUsers"];
        }
    }
} 