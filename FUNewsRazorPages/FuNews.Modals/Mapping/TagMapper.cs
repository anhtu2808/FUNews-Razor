using AutoMapper;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;

namespace FuNews.Modals.Mapping;

public class TagMapper : Profile
{
    public TagMapper()
    {
        CreateMap<Tag, TagResponse>();
        CreateMap<TagRequest, Tag>();
    }
}
