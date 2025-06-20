using AutoMapper;
using FuNews.BLL.Interface;
using FuNews.DAL.Interface;
using FuNews.DAL.Repository;
using FuNews.Modals.DTOs.Response;
using FuNews.Modals.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.BLL.Service
{
    public class CategoryService : BaseService<Category, short>, ICategoryService
    {
        private ICategoryRepository _categoryRepository;
        private IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper) : base(categoryRepository)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<OverviewCategoryResponse>> GetAllCategories(bool? status)
        {
            List<OverviewCategoryResponse> responses;
            if (!status.HasValue)
            {
                responses = _mapper.Map<List<OverviewCategoryResponse>>(await _categoryRepository.GetAllAsync());
            }
            else
            {
                responses = _mapper.Map<List<OverviewCategoryResponse>>(await _categoryRepository.GetAllCategoryByStatus(status));
            }
            return responses;
        }

    }
}
