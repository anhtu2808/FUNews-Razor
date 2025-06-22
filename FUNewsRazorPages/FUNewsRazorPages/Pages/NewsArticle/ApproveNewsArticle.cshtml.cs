using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Response;
using FUNewsRazorPages.SignalR.NewsArticle;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FUNewsRazorPages.Pages.NewsArticle
{
    public class ApproveNewsArticleModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly IHubContext<NewsHub> _hub;

        public ApproveNewsArticleModel(INewsArticleService newsArticleService, IHubContext<NewsHub> hub)
        {
            _newsArticleService = newsArticleService;
            _hub = hub;
        }

        [BindProperty]
        public string? NewsId { get; set; }

        public async Task<IActionResult> OnPostApproveAsync(string id)
        {
            var updatedById = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(updatedById))
            {
                await _newsArticleService.ApproveNews(id, short.Parse(updatedById));
                await _hub.Clients.All.SendAsync("NewsApproved", id);
            }

            return RedirectToPage(); // Reload lại danh sách
        }

        public List<NewsArticleResponse> NewsList { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Lấy các bài chưa duyệt
            NewsList = await _newsArticleService.GetPendingNews();
        }
    }
}
