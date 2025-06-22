using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.DAL.Interface;

public interface ICategoryRepository : IBaseRepository<Category, short>
{
    Task<IEnumerable<Category>> GetAllWithChildrenAsync();
    Task<IEnumerable<Category>> GetChildrenByParentIdAsync(short parentId);
    Task<bool> HasChildrenAsync(short categoryId);
    Task<bool> CanDeleteAsync(short categoryId);
    Task<List<Category>> GetAllCategoryByStatus(bool? isActive);
}