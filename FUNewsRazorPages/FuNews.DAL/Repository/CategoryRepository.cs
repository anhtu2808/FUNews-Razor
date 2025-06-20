using FuNews.DAL.Interface;
using FuNews.Modals.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.DAL.Repository
{
    public class CategoryRepository : BaseRepository<Category, short>, ICategoryRepository
    {
        public CategoryRepository(FuNewsDbContext _context) : base(_context)
        {
        }

        public async Task<List<Category>> GetAllCategoryByStatus(bool? isActive)
        {
            return await _context.Categories
                .Where(c => c.IsActive == isActive)
                .OrderBy(c => c.CategoryName)  
                .ToListAsync();
        }

    }
}
