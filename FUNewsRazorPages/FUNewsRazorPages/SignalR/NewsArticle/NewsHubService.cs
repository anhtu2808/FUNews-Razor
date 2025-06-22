using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Response;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsRazorPages.SignalR.NewsArticle
{
	public class NewsHubService : INewsHubService
	{
		private readonly IHubContext<NewsHub> _hub;
		public NewsHubService(IHubContext<NewsHub> hub)
			=> _hub = hub;

		public Task NewsCreatedAsync(NewsArticleResponse dto)
			=> _hub.Clients.All.SendAsync("NewsCreated", dto);

		public Task NewsUpdatedAsync(NewsArticleResponse dto)
			=> _hub.Clients.All.SendAsync("NewsUpdated", dto);
	}
}
