using FuNews.Modals.DTOs.Response;
using Microsoft.AspNetCore.SignalR;
using System;
namespace FUNewsRazorPages.SignalR.NewsArticle
{
	public class NewsHub : Hub
	{
        public async Task NotifyChange(String action, String id)
        {
            await Clients.All.SendAsync("ReceiveNewsChange", action, id);
        }
    }
}
