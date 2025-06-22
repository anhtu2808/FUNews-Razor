using AutoMapper;
using FuNews.BLL.Interface;
using FuNews.DAL.Interface;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;

namespace FuNews.BLL.Service;

public class TagService : BaseService<Tag, int>, ITagService
{
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;

    public TagService(ITagRepository tagRepository, IMapper mapper) : base(tagRepository)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<List<TagResponse>> GetAllTagsAsync()
    {
        var tags = await _tagRepository.GetAllAsync();
        return _mapper.Map<List<TagResponse>>(tags);
    }

    public async Task<TagResponse?> GetTagByIdAsync(int id)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
        return tag == null ? null : _mapper.Map<TagResponse>(tag);
    }

    public async Task AddTagAsync(TagRequest request)
    {
        var tag = _mapper.Map<Tag>(request);
        await _tagRepository.AddAsync(tag);
    }

    public async Task UpdateTagAsync(int id, TagRequest request)
    {
        var tag = await _tagRepository.GetByIdAsync(id);
        if (tag != null)
        {
            _mapper.Map(request, tag);
            await _tagRepository.UpdateAsync(tag);
        }
    }
}
