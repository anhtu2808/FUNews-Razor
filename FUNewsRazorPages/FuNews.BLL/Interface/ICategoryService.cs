using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;

namespace FuNews.BLL.Interface;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllCategoriesTreeAsync();
    Task<List<CategoryResponse>> GetAllCategoriesFlatAsync();
    Task<List<CategoryResponse>> GetParentCategoriesAsync();
    Task<CategoryResponse?> GetCategoryByIdAsync(short id);
    Task<bool> CanDeleteCategoryAsync(short id);
    Task<bool> AddCategoryAsync(CategoryRequest categoryRequest);
    Task<bool> UpdateCategoryAsync(short id, CategoryRequest categoryRequest);
    Task<bool> DeleteCategoryAsync(short id);
} 