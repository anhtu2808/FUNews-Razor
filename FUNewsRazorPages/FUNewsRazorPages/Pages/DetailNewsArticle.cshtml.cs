using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsRazorPages.Pages
{
    public class DetailNewsArticleModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public DetailNewsArticleModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public NewsArticleResponse? News { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            News = await _newsArticleService.GetNewsById(id);
            if (News == null)
                return NotFound();

            return Page();
        }
    }
}
