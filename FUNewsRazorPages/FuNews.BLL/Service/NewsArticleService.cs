using FuNews.BLL.Interface;
using FuNews.DAL.Interface;
using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.BLL.Service
{
    public class NewsArticleService : BaseService<NewsArticle, String>, INewsArticleService
    {
        private INewsArticleRepository _newsArticleRepository;
        public NewsArticleService(INewsArticleRepository newsArticleRepository) : base(newsArticleRepository) 
        {
            _newsArticleRepository = newsArticleRepository;
        }
    }
}
