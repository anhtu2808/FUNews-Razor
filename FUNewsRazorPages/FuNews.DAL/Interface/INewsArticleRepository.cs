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
        Task<List<NewsArticle>> GetAllNewsByStatusAndCategory(bool? status, short categoryId);
        Task<NewsArticle> GetById(String id);
        Task<int> GetTotalPublic();
        Task<int> GetTotalPending();

        Task<int> CountNewsByCategory(short categoryId);

        Task<int> GetTotalPublic(DateTime start, DateTime end);
        Task<int> GetTotalPending(DateTime start, DateTime end);

        Task<int> CountNewsByCategory(short categoryId, DateTime start, DateTime end);

        Task<List<NewsArticle>> GetPendingNews();
        Task<List<NewsArticle>> GetOwnedNews(short accountId);
    }
}
