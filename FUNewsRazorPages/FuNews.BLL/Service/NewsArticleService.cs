using AutoMapper;
using FuNews.BLL.Interface;
using FuNews.DAL.Interface;
using FuNews.DAL.Repository;
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
        private INewsTagRepository _newsTagRepository;

        public NewsArticleService(INewsArticleRepository newsArticleRepository, IMapper mapper, INewsHubService newsHubService, INewsTagRepository newsTagRepository) : base(newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _mapper = mapper;
            _newHubService = newsHubService;
            _newsTagRepository = newsTagRepository;
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
            await _newsTagRepository.CreateNewsTag(newsArticle.NewsArticleId, request.TagIds);
            var dto = _mapper.Map<NewsArticleResponse>(newsArticle);
           
            return dto;
        }

        public async Task DeleteNews(String id)
        {
            await _newsTagRepository.DeleteNewsTag(id);
            await _newsArticleRepository.DeleteAsync(id);
        }

        public async Task<NewsArticleResponse> GetNewsById(String id)
        {
            return _mapper.Map<NewsArticleResponse>(await _newsArticleRepository.GetById(id));  
        }

        public async Task<UpdateNewsArticleResponse> GetNewsByIdToUpdate(String id)
        {
            var entity = await _newsArticleRepository.GetById(id);

            var response = _mapper.Map<UpdateNewsArticleResponse>(entity);

            response.CategoryId = entity.Category?.CategoryId;
            response.TagIds = entity.NewsTags.Select(nt => nt.TagId).ToList();

            return response;
        }
    }
}
