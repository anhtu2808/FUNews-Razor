using AutoMapper;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.Modals.Mapping
{
    public class NewsArticleMapper : Profile
    {
        public NewsArticleMapper()
        {
            CreateMap<NewsArticle, NewsArticleResponse>()
                .ForMember(dest => dest.UrlThumbnails,
                    opt => opt.MapFrom(src =>string.IsNullOrEmpty(src.UrlThumbnails) ? null : $"/uploads/{src.UrlThumbnails}"))
                .ForMember(dest => dest.CategoryName, opt =>
                    opt.MapFrom(src => src.Category != null ? src.Category.CategoryName : "N/A"))
                .ForMember(dest => dest.TagNames, opt =>
                    opt.MapFrom(src => src.NewsTags != null ? src.NewsTags.Select(nt => nt.Tag.TagName).ToList() : new List<string>()))
                .ForMember(dest => dest.AuthorName, opt =>
                    opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.AccountName : "N/A"));

            CreateMap<CreateNewsArticleRequest, NewsArticle>()
                .ForMember(dest => dest.UrlThumbnails, opt => opt.MapFrom(src => src.UrlThumbnailsPath));
            CreateMap<NewsArticle, UpdateNewsArticleResponse>();
            CreateMap<UpdateNewsArticleRequest, NewsArticle>()
                   .ForMember(dest => dest.NewsArticleId, opt => opt.Ignore());

        }
    }
}
