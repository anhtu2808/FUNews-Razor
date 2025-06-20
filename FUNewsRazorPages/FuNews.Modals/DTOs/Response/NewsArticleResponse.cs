using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.Modals.DTOs.Response
{
    public class NewsArticleResponse
    {
        public string NewsArticleId { get; set; }
        public string? NewsTitle { get; set; }
        public string Headline { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public bool? NewsStatus { get; set; }
    }
}
