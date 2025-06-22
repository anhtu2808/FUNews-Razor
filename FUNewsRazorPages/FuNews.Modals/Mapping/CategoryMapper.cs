using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;

namespace FuNews.Modals.Mapping;

public static class CategoryMapper
{
    public static CategoryResponse ToResponse(this Category category, int level = 0, string? parentCategoryName = null)
    {
        return new CategoryResponse
        {
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            CategoryDescription = category.CategoryDescription,
            ParentCategoryId = category.ParentCategoryId,
            IsActive = category.IsActive,
            ParentCategoryName = parentCategoryName,
            Level = level,
            HasChildren = category.SubCategories?.Any() == true
        };
    }

    public static Category ToEntity(this CategoryRequest request)
    {
        return new Category
        {
            CategoryName = request.CategoryName,
            CategoryDescription = request.CategoryDescription,
            ParentCategoryId = request.ParentCategoryId,
            IsActive = request.IsActive
        };
    }

    public static void UpdateEntity(this CategoryRequest request, Category category)
    {
        category.CategoryName = request.CategoryName;
        category.CategoryDescription = request.CategoryDescription;
        category.ParentCategoryId = request.ParentCategoryId;
        category.IsActive = request.IsActive;
    }

    public static List<CategoryResponse> ToTreeStructure(this IEnumerable<Category> categories)
    {
        var categoryList = categories.ToList();
        var result = new List<CategoryResponse>();

        // Get root categories (no parent)
        var rootCategories = categoryList.Where(c => c.ParentCategoryId == null).ToList();

        foreach (var rootCategory in rootCategories)
        {
            var categoryResponse = rootCategory.ToResponse();
            AddChildren(categoryResponse, categoryList, 0);
            result.Add(categoryResponse);
        }

        return result;
    }

    public static List<CategoryResponse> ToFlatTreeStructure(this IEnumerable<Category> categories)
    {
        var categoryList = categories.ToList();
        var result = new List<CategoryResponse>();

        // Get root categories (no parent)
        var rootCategories = categoryList.Where(c => c.ParentCategoryId == null).ToList();

        foreach (var rootCategory in rootCategories)
        {
            AddToFlatList(rootCategory, categoryList, result, 0, null);
        }

        return result;
    }

    private static void AddChildren(CategoryResponse parent, List<Category> allCategories, int level)
    {
        var children = allCategories.Where(c => c.ParentCategoryId == parent.CategoryId).ToList();

        if (children.Any())
        {
            parent.Children = new List<CategoryResponse>();
            foreach (var child in children)
            {
                var childResponse = child.ToResponse(level + 1, parent.CategoryName);
                parent.Children.Add(childResponse);
                AddChildren(childResponse, allCategories, level + 1);
            }
        }
    }

    private static void AddToFlatList(Category category, List<Category> allCategories, List<CategoryResponse> result, int level, string? parentName)
    {
        var categoryResponse = category.ToResponse(level, parentName);
        result.Add(categoryResponse);

        // Add children recursively
        var children = allCategories.Where(c => c.ParentCategoryId == category.CategoryId).ToList();
        foreach (var child in children)
        {
            AddToFlatList(child, allCategories, result, level + 1, category.CategoryName);
        }
    }
}