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

        public Task<int> CountNewsByCategory(short categoryId)
        {
            return _context.NewsArticles
                .Where(na => na.CategoryId == categoryId && na.NewsStatus == true)
                .CountAsync();
        }

        public Task<int> CountNewsByCategory(short categoryId, DateTime start, DateTime end)
        {
            return _context.NewsArticles
               .Where(na => na.CategoryId == categoryId && na.NewsStatus == true && (na.CreatedDate >= start && na.CreatedDate <= end))
               .CountAsync();
        }

        public async Task<List<NewsArticle>> GetAllNewsByStatusAndCategory(bool? status, short categoryId)
        {
            return await _context.NewsArticles
                    .Include(n => n.Category)
                    .Include(n => n.CreatedBy)
                    .Include(n => n.NewsTags)
                    .Where(na => na.NewsStatus == status && na.CategoryId == categoryId)
                    .OrderByDescending(na => na.CreatedDate)
                    .ToListAsync();
        }

        public async Task<NewsArticle> GetById(String id)
        {
            return await _context.NewsArticles
                    .Include(n => n.Category).Include(n => n.CreatedBy).Include(n => n.NewsTags).ThenInclude(nt => nt.Tag)
                    .FirstAsync(na => na.NewsArticleId == id);
        }

        public Task<int> GetTotalPending()
        {
            return _context.NewsArticles
                .Where(na => na.NewsStatus == false)
                .CountAsync();
        }

        public Task<int> GetTotalPending(DateTime start, DateTime end)
        {
            return _context.NewsArticles
                 .Where(na => na.NewsStatus == false && (na.CreatedDate >= start && na.CreatedDate <= end))
                 .CountAsync();
        }

        public Task<int> GetTotalPublic()
        {
            return _context.NewsArticles
              .Where(na => na.NewsStatus == true)
              .CountAsync();
        }

        public Task<int> GetTotalPublic(DateTime start, DateTime end)
        {
            return _context.NewsArticles
             .Where(na => na.NewsStatus == true && (na.CreatedDate >= start && na.CreatedDate <= end))
             .CountAsync();
        }

        public async Task<List<NewsArticle>> GetPendingNews()
        {
            return await _context.NewsArticles
            .Include(n => n.Category)
            .Include(n => n.CreatedBy)
            .Include(n => n.NewsTags)
            .Where(na => na.NewsStatus == false)
            .OrderByDescending(na => na.CreatedDate)
            .ToListAsync();
        }

        public async Task<List<NewsArticle>> GetOwnedNews(short accountId)
        {
            return await _context.NewsArticles
.Include(n => n.Category)
.Include(n => n.CreatedBy)
.Include(n => n.NewsTags)
.Where(na => na.CreatedById == accountId)
.OrderByDescending(na => na.CreatedDate)
.ToListAsync();
        }

        public async Task<List<NewsArticle>> GetAllNews()
        {
			return await _context.NewsArticles
		.Include(n => n.Category)
		.Include(n => n.CreatedBy)
		.Include(n => n.NewsTags)
		.OrderByDescending(na => na.CreatedDate)
		.ToListAsync();
		}
	}
}
