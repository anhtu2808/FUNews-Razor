using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Response;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsRazorPages.Pages.Tags;

public class IndexModel : PageModel
{
    private readonly ITagService _tagService;

    public IndexModel(ITagService tagService)
    {
        _tagService = tagService;
    }

    public List<TagResponse> Tags { get; set; } = new();

    public async Task OnGetAsync()
    {
        Tags = await _tagService.GetAllTagsAsync();
    }
}
