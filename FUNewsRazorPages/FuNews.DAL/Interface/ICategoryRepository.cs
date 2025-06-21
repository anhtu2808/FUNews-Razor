using FuNews.Modals.Entity;

namespace FuNews.DAL.Interface;

public interface ICategoryRepository : IBaseRepository<Category, short>
{
    Task<IEnumerable<Category>> GetAllWithChildrenAsync();
    Task<IEnumerable<Category>> GetChildrenByParentIdAsync(short parentId);
    Task<bool> HasChildrenAsync(short categoryId);
    Task<bool> CanDeleteAsync(short categoryId);
} 