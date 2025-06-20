using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.DAL.Interface
{
    public interface INewsArticleRepository : IBaseRepository<NewsArticle, String>
    {
        Task<List<NewsArticle>> GetAllNewsByStatus(bool? status);

    }
}
