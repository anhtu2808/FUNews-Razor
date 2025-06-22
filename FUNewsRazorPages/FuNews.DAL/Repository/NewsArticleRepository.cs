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
    public class NewsArticleRepository : BaseRepository<NewsArticle, String>, INewsArticleRepository
    {
        public NewsArticleRepository(FuNewsDbContext _context) : base(_context)
        {
        }

        public async Task<List<NewsArticle>> GetAllNewsByStatus(bool? status)
        {
            return await _context.NewsArticles
                    .Include(n => n.Category).Include(n => n.CreatedBy).Include(n => n.NewsTags)
                    .Where(na => na.NewsStatus == status)
                    .OrderByDescending(na => na.CreatedDate)
                    .ToListAsync();
        }

    }
}
