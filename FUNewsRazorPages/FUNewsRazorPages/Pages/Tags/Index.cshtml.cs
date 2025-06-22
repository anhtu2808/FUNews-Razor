using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
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

    [BindProperty]
    public TagRequest Tag { get; set; } = new();

    public async Task OnGetAsync()
    {
        Tags = await _tagService.GetAllTagsAsync();
    }

    // Handle Create
    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        await _tagService.AddTagAsync(Tag);
        return RedirectToPage();
    }

    // Handle Edit
    public async Task<IActionResult> OnPostEditAsync(int id)
    {
        if (!ModelState.IsValid)
            return Page();

        await _tagService.UpdateTagAsync(id, Tag);
        return RedirectToPage();
    }

    // Handle Delete
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _tagService.DeleteAsync(id);
        return RedirectToPage();
    }
}
