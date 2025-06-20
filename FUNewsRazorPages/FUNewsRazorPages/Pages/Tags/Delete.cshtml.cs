using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsRazorPages.Pages.Tags;

public class DeleteModel : PageModel
{
    private readonly ITagService _tagService;

    public DeleteModel(ITagService tagService)
    {
        _tagService = tagService;
    }

    public TagResponse? Tag { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Tag = await _tagService.GetTagByIdAsync(id);
        if (Tag == null)
            return RedirectToPage("Index");
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        await _tagService.DeleteAsync(id);
        return RedirectToPage("Index");
    }
}
