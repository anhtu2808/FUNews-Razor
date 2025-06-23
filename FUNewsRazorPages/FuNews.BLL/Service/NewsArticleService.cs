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
        private INewsTagRepository _newsTagRepository;

        public NewsArticleService(INewsArticleRepository newsArticleRepository, IMapper mapper, INewsTagRepository newsTagRepository) : base(newsArticleRepository)
        {
            _newsArticleRepository = newsArticleRepository;
            _mapper = mapper;
            _newsTagRepository = newsTagRepository;
        }

        public async Task<List<NewsArticleResponse>> GetAllNews(bool? status, short? categoryId)
        {
            List<NewsArticleResponse> responses;
            if ((!status.HasValue && !categoryId.HasValue) || (status.HasValue && !categoryId.HasValue))
            {
                responses = _mapper.Map<List<NewsArticleResponse>>(await _newsArticleRepository.GetAllNews());
            }
            else
            {
                responses = _mapper.Map<List<NewsArticleResponse>>(await _newsArticleRepository.GetAllNewsByStatusAndCategory(status, categoryId.Value));
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

        public async Task<NewsArticleResponse> UpdateNews(UpdateNewsArticleRequest request)
        {
            NewsArticle newsArticle = await _newsArticleRepository.GetById(request.NewsArticleId);
            await _newsTagRepository.UpdateNewsTag(newsArticle.NewsArticleId, request.TagIds);
            await _newsArticleRepository.UpdateAsync(_mapper.Map(request, newsArticle));
            return _mapper.Map<NewsArticleResponse>(newsArticle);
        }

        public async Task<List<NewsArticleResponse>> GetPendingNews()
        {
            return _mapper.Map<List<NewsArticleResponse>>(await _newsArticleRepository.GetPendingNews());
        }

        public async Task ApproveNews(String id, short accountId)
        {
            var news = await _newsArticleRepository.GetById(id);
            news.NewsStatus = true;
            news.ModifiedDate = DateTime.Now;
            news.UpdatedById = accountId;
            await _newsArticleRepository.UpdateAsync(news);
        }

        public async Task<List<NewsArticleResponse>> GetOwnedNews(short accountId)
        {
            return _mapper.Map<List<NewsArticleResponse>>(await _newsArticleRepository.GetOwnedNews(accountId));
        }

    }
}
