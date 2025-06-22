using FuNews.DAL.Interface;
using FuNews.Modals.Entity;

namespace FuNews.DAL.Repository;

public class TagRepository : BaseRepository<Tag, int>, ITagRepository
{
    public TagRepository(FuNewsDbContext context) : base(context)
    {
    }
}
