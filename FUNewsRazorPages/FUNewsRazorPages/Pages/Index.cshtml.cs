using FuNews.BLL.Interface;
using FuNews.BLL.Service;
using FuNews.Modals.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsRazorPages.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;

        public IndexModel(ILogger<IndexModel> logger, INewsArticleService newsArticleService, ICategoryService categoryService)
        {
            _logger = logger;
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
        }

        public List<NewsArticleResponse> NewsList { get; set; } = new();
        public List<OverviewCategoryResponse> Categories { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public short? SelectedCategoryId { get; set; }

        public async Task OnGetAsync()
        {
            NewsList = await _newsArticleService.GetAllNews(true, SelectedCategoryId);
            Categories = await _categoryService.GetAllCategories(true);

        }
    }
}
