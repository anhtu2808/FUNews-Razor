using AutoMapper;
using FuNews.BLL.Interface;
using FuNews.DAL.Interface;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;
using Microsoft.AspNetCore.SignalR;
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
        private INewsHubService _newHubService;

        public NewsArticleService(INewsArticleRepository newsArticleRepository, IMapper mapper, INewsHubService newsHubService) : base(newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _mapper = mapper;
            _newHubService = newsHubService;
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

        public async Task<NewsArticleResponse> CreateNews(CreateNewsArticleRequest request)
        {
            NewsArticle newsArticle = _mapper.Map<NewsArticle>(request);

            newsArticle.NewsArticleId = Guid.NewGuid().ToString("N").Substring(0, 20);
            newsArticle.ModifiedDate = DateTime.Now;
            newsArticle.CreatedDate = DateTime.Now;
            newsArticle.NewsStatus = true;
            newsArticle.UpdatedById = 1;
            newsArticle.CreatedById = 1;

            await _newsArticleRepository.AddAsync(newsArticle);
            var dto = _mapper.Map<NewsArticleResponse>(newsArticle);
           
            return dto;
        }
    }
}
