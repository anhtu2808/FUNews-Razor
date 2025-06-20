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
    }
}
