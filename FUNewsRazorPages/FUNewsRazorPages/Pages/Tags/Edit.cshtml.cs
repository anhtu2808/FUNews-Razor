using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsRazorPages.Pages.Tags;

public class EditModel : PageModel
{
    private readonly ITagService _tagService;

    public EditModel(ITagService tagService)
    {
        _tagService = tagService;
    }

    [BindProperty]
    public TagRequest Tag { get; set; } = new();

    public int Id { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var tag = await _tagService.GetTagByIdAsync(id);
        if (tag == null)
            return RedirectToPage("Index");
        Id = tag.TagId;
        Tag.TagName = tag.TagName ?? string.Empty;
        Tag.Note = tag.Note;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
            return Page();
        await _tagService.UpdateTagAsync(id, Tag);
        return RedirectToPage("Index");
    }
}
