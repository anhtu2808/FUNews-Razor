using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.DAL.Interface
{
    public interface INewsTagRepository
    {

        Task CreateNewsTag(String id, List<int> tagIds);
        Task DeleteNewsTag(String id);
        Task UpdateNewsTag(String id, List<int> tagIds);
        Task<List<NewsTag>> GetAllByNewsIdAsync(String id);
        Task<int> CountNewsByTag(int tagId);
        Task<int> CountNewsByTag(int tagId, DateTime start, DateTime end);
    }
}
