using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.BLL.Interface
{
    public interface ICategoryService : IBaseService<Category, short>
    {
        Task<List<OverviewCategoryResponse>> GetAllCategories(bool? status);
        Task<List<CategoryResponse>> GetAllCategoriesTreeAsync();
        Task<List<CategoryResponse>> GetAllCategoriesFlatAsync();
        Task<List<CategoryResponse>> GetParentCategoriesAsync();
        Task<CategoryResponse?> GetCategoryByIdAsync(short id);
        Task<bool> CanDeleteCategoryAsync(short id);
        Task<bool> AddCategoryAsync(CategoryRequest categoryRequest);
        Task<bool> UpdateCategoryAsync(short id, CategoryRequest categoryRequest);
        Task<bool> DeleteCategoryAsync(short id);
    }
}
