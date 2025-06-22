using FuNews.BLL.Interface;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsRazorPages.Pages.Categories;

public class IndexModel : PageModel
{
    private readonly ICategoryService _categoryService;

    public IndexModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public List<CategoryResponse> Categories { get; set; } = new();
    public List<CategoryResponse> ParentCategories { get; set; } = new();

    [BindProperty]
    public CategoryRequest Category { get; set; } = new();

    public async Task OnGetAsync()
    {
        Categories = await _categoryService.GetAllCategoriesFlatAsync();
        ParentCategories = await _categoryService.GetParentCategoriesAsync();
    }

    // Handle Create
    public async Task<IActionResult> OnPostCreateAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var success = await _categoryService.AddCategoryAsync(Category);
        if (!success)
        {
            ModelState.AddModelError("", "Failed to create category. Please check if parent category is valid.");
            return Page();
        }

        return RedirectToPage();
    }

    // Handle Edit
    public async Task<IActionResult> OnPostEditAsync(short id)
    {
        if (!ModelState.IsValid)
            return Page();

        var success = await _categoryService.UpdateCategoryAsync(id, Category);
        if (!success)
        {
            ModelState.AddModelError("", "Failed to update category. Please check if parent category is valid and not creating circular reference.");
            return Page();
        }

        return RedirectToPage();
    }

    // Handle Delete
    public async Task<IActionResult> OnPostDeleteAsync(short id)
    {
        var canDelete = await _categoryService.CanDeleteCategoryAsync(id);
        if (!canDelete)
        {
            ModelState.AddModelError("", "Cannot delete category that has children or is being used.");
            return Page();
        }

        var success = await _categoryService.DeleteCategoryAsync(id);
        if (!success)
        {
            ModelState.AddModelError("", "Failed to delete category.");
            return Page();
        }

        return RedirectToPage();
    }
} 