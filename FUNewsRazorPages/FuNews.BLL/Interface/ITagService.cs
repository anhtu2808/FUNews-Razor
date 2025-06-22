using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;

namespace FuNews.BLL.Interface;

public interface ITagService : IBaseService<Tag, int>
{
    Task<List<TagResponse>> GetAllTagsAsync();
    Task<TagResponse?> GetTagByIdAsync(int id);
    Task AddTagAsync(TagRequest request);
    Task UpdateTagAsync(int id, TagRequest request);
}
