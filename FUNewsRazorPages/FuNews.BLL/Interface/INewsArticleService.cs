using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;
using FuNews.Modals.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.BLL.Interface
{
    public interface INewsArticleService : IBaseService<NewsArticle, String>
    {
        Task<List<NewsArticleResponse>> GetAllNews(bool? status);

		Task<NewsArticleResponse> CreateNews(CreateNewsArticleRequest request);

        Task DeleteNews(String id);

        Task<NewsArticleResponse> GetNewsById(String id);
        Task<UpdateNewsArticleResponse> GetNewsByIdToUpdate(String id);

    }
}
