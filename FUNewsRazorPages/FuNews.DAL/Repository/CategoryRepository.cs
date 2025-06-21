using FuNews.DAL.Interface;
using FuNews.Modals.Entity;
using Microsoft.EntityFrameworkCore;


namespace FuNews.DAL.Repository;

public class CategoryRepository : BaseRepository<Category,short>, ICategoryRepository
{
    public CategoryRepository(FuNewsDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Category>> GetAllWithChildrenAsync()
    {
        return await _context.Set<Category>()
            .Include(c => c.SubCategories)
            .Include(c => c.ParentCategory)
            .OrderBy(c => c.ParentCategoryId ?? 0)
            .ThenBy(c => c.CategoryName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetChildrenByParentIdAsync(short parentId)
    {
        return await _context.Set<Category>()
            .Where(c => c.ParentCategoryId == parentId)
            .OrderBy(c => c.CategoryName)
            .ToListAsync();
    }

    public async Task<bool> HasChildrenAsync(short categoryId)
    {
        return await _context.Set<Category>()
            .AnyAsync(c => c.ParentCategoryId == categoryId);
    }

    public async Task<bool> CanDeleteAsync(short categoryId)
    {
        // Check if category has children
        var hasChildren = await HasChildrenAsync(categoryId);
        if (hasChildren)
            return false;

        // Check if category is used by news articles (if NewsArticle entity has CategoryId)
        // This would need to be implemented based on your NewsArticle entity structure
        // For now, just check children
        return true;
    }
} 