using FuNews.BLL.Interface;
using FuNews.BLL.Service;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FUNewsRazorPages.SignalR.NewsArticle;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace FUNewsRazorPages.Pages.NewsArticle
{
    public class ManageNewsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<IndexModel> _logger;
		private readonly IHubContext<NewsHub> _hub;
		public ManageNewsModel(ILogger<IndexModel> logger, INewsArticleService newsArticleService, ICategoryService categoryService, IHubContext<NewsHub> hub)
        {
            _newsArticleService = newsArticleService;
            _logger = logger;
            _categoryService = categoryService;
            _hub = hub;
        }

        public List<NewsArticleResponse> NewsList { get; set; } = new();
        [BindProperty]
        public CreateNewsArticleRequest Input { get; set; } = new();
        public List<SelectListItem> CategoryOptions { get; set; } = new();
        public async Task OnGetAsync()
        {
            NewsList = await _newsArticleService.GetAllNews(true);
            var categories = await _categoryService.GetAllCategories(true);
            CategoryOptions = new List<SelectListItem>();
            foreach (var cat in categories)
            {
                CategoryOptions.Add(new SelectListItem
                {
                    Value = cat.CategoryId.ToString(),
                    Text = cat.CategoryName
                });
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            await _newsArticleService.CreateNews(Input);
			await _hub.Clients.All.SendAsync("ReceiveNewsChange", "created");
			return RedirectToPage("ManageNews");
        }

		public async Task<IActionResult> OnPostDeleteAsync(String id)
		{
			await _newsArticleService.DeleteAsync(id);
			await _hub.Clients.All.SendAsync("ReceiveNewsChange", "deleted");
			return RedirectToPage("ManageNews");
		}
	}
}
