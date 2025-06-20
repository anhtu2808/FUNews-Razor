using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly INewsArticleService _newsArticleService;

        public IndexModel(ILogger<IndexModel> logger, INewsArticleService newsArticleService)
        {
            _logger = logger;
            _newsArticleService = newsArticleService;
        }

        public List<NewsArticleResponse> NewsList { get; set; } = new();

        public async Task OnGetAsync()
        {
            NewsList = await _newsArticleService.GetAllNews(null);
        }
    }
}
