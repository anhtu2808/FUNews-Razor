﻿using AutoMapper;
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
              opt => opt.MapFrom(src =>
                  string.IsNullOrEmpty(src.UrlThumbnails)
                      ? null
                      : $"/uploads/{src.UrlThumbnails}"
              ));
        }
    }
}
