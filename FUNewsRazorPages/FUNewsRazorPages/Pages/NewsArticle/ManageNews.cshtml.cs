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
        private readonly ITagService _tagService;
        private readonly IWebHostEnvironment _env;
		public ManageNewsModel(ILogger<IndexModel> logger, INewsArticleService newsArticleService, ICategoryService categoryService, IHubContext<NewsHub> hub, ITagService tagService, IWebHostEnvironment env)
        {
            _newsArticleService = newsArticleService;
            _logger = logger;
            _categoryService = categoryService;
            _hub = hub;
            _tagService = tagService;
            _env = env;
        }

        public List<NewsArticleResponse> NewsList { get; set; } = new();
        [BindProperty]
        public CreateNewsArticleRequest Input { get; set; } = new();
        public List<SelectListItem> CategoryOptions { get; set; } = new();
        public List<TagResponse> AvailableTags { get; set; } = new();
        [BindProperty]
        public UpdateNewsArticleRequest EditInput { get; set; } = new();
        public async Task OnGetAsync()
        {
            NewsList = await _newsArticleService.GetAllNews(true);
            var categories = await _categoryService.GetAllCategories(true);
            AvailableTags = await _tagService.GetAllTagsAsync();
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

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }
            if (Input.UrlThumbnailsFile != null)
            {
                var savedPath = await SaveImageAsync(Input.UrlThumbnailsFile, _env.WebRootPath);
                Input.UrlThumbnailsPath = savedPath; 
            }


            await _newsArticleService.CreateNews(Input);
			await _hub.Clients.All.SendAsync("ReceiveNewsChange", "created");
			return RedirectToPage("ManageNews");
        }

		public async Task<IActionResult> OnPostDeleteAsync(String id)
		{
			await _newsArticleService.DeleteNews(id);
			await _hub.Clients.All.SendAsync("ReceiveNewsChange", "deleted");
			return RedirectToPage("ManageNews");
		}

        public async Task<JsonResult> OnGetGetNews(string id)
        {
            var news = await _newsArticleService.GetNewsByIdToUpdate(id);

            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            };

            return new JsonResult(news, options);
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }
            if (EditInput.UrlThumbnailsFile != null)
            {
                var savedFileName = await SaveImageAsync(EditInput.UrlThumbnailsFile, _env.WebRootPath);
                EditInput.UrlThumbnails = savedFileName; 
            }
            await _newsArticleService.UpdateNews(EditInput);
            await _hub.Clients.All.SendAsync("ReceiveNewsChange", "edited");
            return RedirectToPage("ManageNews");
        }

        private async Task<string> SaveImageAsync(IFormFile file, string webRootPath, string subFolder = "uploads")
        {
            if (file == null || file.Length == 0) return null;

            var uploadsFolder = Path.Combine(webRootPath, subFolder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
