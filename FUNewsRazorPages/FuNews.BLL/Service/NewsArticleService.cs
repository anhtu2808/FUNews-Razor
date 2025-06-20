using AutoMapper;
using FuNews.BLL.Interface;
using FuNews.DAL.Interface;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.BLL.Service
{
    public class NewsArticleService : BaseService<NewsArticle, String>, INewsArticleService
    {
        private INewsArticleRepository _newsArticleRepository;
        private IMapper _mapper;
        public NewsArticleService(INewsArticleRepository newsArticleRepository, IMapper mapper) : base(newsArticleRepository) 
        {
            _newsArticleRepository = newsArticleRepository;
            _mapper = mapper;
        }

        public async Task<List<NewsArticleResponse>> GetAllNews(bool? status)
        {
            List<NewsArticleResponse> responses;
            if (!status.HasValue)
            {
                responses = _mapper.Map<List<NewsArticleResponse>>(await _newsArticleRepository.GetAllAsync());
            }
            else 
            {
                responses = _mapper.Map<List<NewsArticleResponse>>(await _newsArticleRepository.GetAllNewsByStatus(status));
            }
            return responses;
        }
    }
}
