using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.DAL.Interface
{
    public interface ICategoryRepository : IBaseRepository<Category, short>
    {
        Task<List<Category>> GetAllCategoryByStatus(bool? isActive);

    }
}
