using FuNews.BLL.Interface;
using FuNews.DAL.Interface;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Mapping;

namespace FuNews.BLL.Service;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryResponse>> GetAllCategoriesTreeAsync()
    {
        var categories = await _categoryRepository.GetAllWithChildrenAsync();
        return categories.ToTreeStructure();
    }

    public async Task<List<CategoryResponse>> GetAllCategoriesFlatAsync()
    {
        var categories = await _categoryRepository.GetAllWithChildrenAsync();
        return categories.ToFlatTreeStructure();
    }

    public async Task<List<CategoryResponse>> GetParentCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllWithChildrenAsync();
        return categories
            .Where(c => c.IsActive == true)
            .Select(c => c.ToResponse())
            .OrderBy(c => c.CategoryName)
            .ToList();
    }

    public async Task<CategoryResponse?> GetCategoryByIdAsync(short id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);
        return category?.ToResponse(0, category.ParentCategory?.CategoryName);
    }

    public async Task<bool> CanDeleteCategoryAsync(short id)
    {
        return await _categoryRepository.CanDeleteAsync(id);
    }

    public async Task<bool> AddCategoryAsync(CategoryRequest categoryRequest)
    {
        try
        {
            // Validate parent category exists if specified
            if (categoryRequest.ParentCategoryId.HasValue)
            {
                var parentCategory = await _categoryRepository.GetByIdAsync(categoryRequest.ParentCategoryId.Value);
                if (parentCategory == null || parentCategory.IsActive != true)
                {
                    return false;
                }
            }

            var category = categoryRequest.ToEntity();
            await _categoryRepository.AddAsync(category);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateCategoryAsync(short id, CategoryRequest categoryRequest)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                return false;

            // Validate parent category if specified and different from current
            if (categoryRequest.ParentCategoryId.HasValue)
            {
                // Prevent setting parent to self or descendants
                if (categoryRequest.ParentCategoryId == id)
                    return false;

                var parentCategory = await _categoryRepository.GetByIdAsync(categoryRequest.ParentCategoryId.Value);
                if (parentCategory == null || parentCategory.IsActive != true)
                    return false;

                // Check if new parent is not a descendant of current category
                if (await IsDescendant(id, categoryRequest.ParentCategoryId.Value))
                    return false;
            }

            categoryRequest.UpdateEntity(category);
            await _categoryRepository.UpdateAsync(category);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteCategoryAsync(short id)
    {
        try
        {
            var canDelete = await CanDeleteCategoryAsync(id);
            if (!canDelete)
                return false;

            await _categoryRepository.DeleteAsync(id);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> IsDescendant(short categoryId, short potentialParentId)
    {
        var children = await _categoryRepository.GetChildrenByParentIdAsync(categoryId);
        
        foreach (var child in children)
        {
            if (child.CategoryId == potentialParentId)
                return true;
            
            if (await IsDescendant(child.CategoryId, potentialParentId))
                return true;
        }
        
        return false;
    }
} 