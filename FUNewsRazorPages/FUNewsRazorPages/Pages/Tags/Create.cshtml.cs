using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsRazorPages.Pages.Tags;

public class CreateModel : PageModel
{
    private readonly ITagService _tagService;

    public CreateModel(ITagService tagService)
    {
        _tagService = tagService;
    }

    [BindProperty]
    public TagRequest Tag { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        await _tagService.AddTagAsync(Tag);
        return RedirectToPage("Index");
    }
}
