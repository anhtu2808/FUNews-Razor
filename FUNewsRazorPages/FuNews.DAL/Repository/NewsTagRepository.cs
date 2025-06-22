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
    public class NewsTagRepository : INewsTagRepository
    {
        protected readonly FuNewsDbContext _context;
        protected readonly DbSet<NewsTag> _dbSet;

        public NewsTagRepository(FuNewsDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<NewsTag>();
        }

        public async Task CreateNewsTag(String id, List<int> tagIds)
        {
            foreach (var tag in tagIds)
            {
                NewsTag newsTag = new()
                {
                    NewsArticleId = id,
                    TagId = tag
                };
                await _dbSet.AddAsync(newsTag);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteNewsTag(String id)
        {
            var tagsToRemove = await GetAllByNewsIdAsync(id);
            if (tagsToRemove != null)
            {
                _context.NewsTags.RemoveRange(tagsToRemove);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<NewsTag>> GetAllByNewsIdAsync(String id)
        {
            return await _context.NewsTags
                .Where(nt => nt.NewsArticleId == id)
                .ToListAsync();
        }

        public async Task UpdateNewsTag(String id, List<int> tagIds)
        {
            await DeleteNewsTag(id);
            foreach (var tag in tagIds)
            {
                NewsTag newsTag = new()
                {
                    NewsArticleId = id,
                    TagId = tag
                };
                await _dbSet.AddAsync(newsTag);
            }
            await _context.SaveChangesAsync();
        }

    }
}
