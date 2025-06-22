using FuNews.BLL.Interface;
using FuNews.DAL.Interface;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.BLL.Service
{
    public class DashboardService : IDashboardService
    {
        private INewsArticleRepository _newsArticleRepository;
        private ICategoryRepository _categoryRepository;
        private ITagRepository _tagRepository;
        private INewsTagRepository _newsTagRepository;

        public DashboardService(INewsArticleRepository newsArticleRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository, INewsTagRepository newsTagRepository)
        {
            _categoryRepository = categoryRepository;
            _newsArticleRepository = newsArticleRepository;
            _tagRepository = tagRepository;
            _newsTagRepository = newsTagRepository;
        }

        public async Task<NewsDashboardResponse> GetNewsDashboard(NewsDashboardRequest request)
        {
            int TotalPublic = 0;
            int TotalPending = 0;
            List<CategoryDashboardResponse> categoryResponse;
            List<TagDashboardResponse> tagResponse;
            List<Category> categories;
            List<Tag> tags;
            if (!request.startDate.HasValue && !request.endDate.HasValue)
            {
                TotalPublic = await _newsArticleRepository.GetTotalPublic();
                TotalPending = await _newsArticleRepository.GetTotalPending();
                categoryResponse = new();
                tagResponse = new();
                categories = await _categoryRepository.GetAllCategoryByStatus(true);
                foreach (Category category in categories) 
                {
                    int numberOfNews = await _newsArticleRepository.CountNewsByCategory(category.CategoryId);
                    CategoryDashboardResponse categoryDashboard = new()
                    {
                        CategoryCounts = numberOfNews,
                        CategoryNames = category.CategoryName
                    };
                    categoryResponse.Add(categoryDashboard);
                }
                tags = await _tagRepository.GetAllAsync();
                foreach (Tag tag in tags) 
                {
                    int numberOfNews = await _newsTagRepository.CountNewsByTag(tag.TagId);
                    TagDashboardResponse tagDashboard = new()
                    {
                        TagCounts = numberOfNews,
                        TagNames = tag.TagName
                    };
                    tagResponse.Add(tagDashboard);
                }
                return new()
                {
                    TotalPublic = TotalPublic,
                    TotalPending = TotalPending,
                    CategoryDashboard = categoryResponse,
                    TagDashboard = tagResponse
                };
            }
            DateTime startDate;
            DateTime endDate;
            if (request.startDate.HasValue && !request.endDate.HasValue)
            {
                startDate = request.startDate.Value.Date;
                endDate = startDate.AddDays(1).AddTicks(-1); // kết thúc trong ngày
            }
            else if (!request.startDate.HasValue && request.endDate.HasValue)
            {
                endDate = request.endDate.Value.Date.AddDays(1).AddTicks(-1);
                startDate = request.endDate.Value.Date;
            }
            else
            {
                startDate = request.startDate?.Date ?? DateTime.Today;
                endDate = request.endDate?.Date.AddDays(1).AddTicks(-1)
                          ?? DateTime.Today.AddDays(1).AddTicks(-1);
            }

            TotalPublic = await _newsArticleRepository.GetTotalPublic(startDate, endDate);
            TotalPending = await _newsArticleRepository.GetTotalPending(startDate, endDate);
            categoryResponse = new();
            tagResponse = new();
            categories = await _categoryRepository.GetAllCategoryByStatus(true);
            foreach (Category category in categories)
            {
                int numberOfNews = await _newsArticleRepository.CountNewsByCategory(category.CategoryId, startDate, endDate);
                CategoryDashboardResponse categoryDashboard = new()
                {
                    CategoryCounts = numberOfNews,
                    CategoryNames = category.CategoryName
                };
                categoryResponse.Add(categoryDashboard);
            }
            tags = await _tagRepository.GetAllAsync();
            foreach (Tag tag in tags)
            {
                int numberOfNews = await _newsTagRepository.CountNewsByTag(tag.TagId, startDate, endDate);
                TagDashboardResponse tagDashboard = new()
                {
                    TagCounts = numberOfNews,
                    TagNames = tag.TagName
                };
                tagResponse.Add(tagDashboard);
            }
            return new()
            {
                TotalPublic = TotalPublic,
                TotalPending = TotalPending,
                CategoryDashboard = categoryResponse,
                TagDashboard = tagResponse
            };
        }

    }
}
