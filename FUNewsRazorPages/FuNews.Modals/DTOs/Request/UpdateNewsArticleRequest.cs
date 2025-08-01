﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.Modals.DTOs.Request
{
    public class UpdateNewsArticleRequest
    {
        public string? NewsArticleId {  get; set; }
        public string? NewsTitle { get; set; }
        public string? Headline { get; set; }

        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public IFormFile? UrlThumbnailsFile { get; set; } 
        public string? UrlThumbnails { get; set; }
        public short? CategoryId { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
