using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using VegetableShop.Api.Common;
using VegetableShop.Api.Data.EF;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Category;

namespace VegetableShop.Api.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public CategoryService(AppDbContext appDbContext, IMapper mapper)
        {
            _appDbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<CreateResponse> CreateAsync(CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto is null)
            {
                throw new BadHttpRequestException(Exceptions.BadRequest);
            }
            if (_appDbContext.Categories.FirstOrDefault(x => x.Name.Equals(createCategoryDto.Name)) is not null)
            {
                return new CreateResponse(Exceptions.CategoryNameExist);
            }
            var category = _mapper.Map<Category>(createCategoryDto);
            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();
            return new CreateResponse(_mapper.Map<CategoryDto>(category), Messages.CreateSuccess);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = _appDbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (category is null)
            {
                return false;
            }
            var init = _appDbContext.Database.CreateExecutionStrategy();
            await init.ExecuteAsync(async () =>
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    _appDbContext.Categories.Remove(category);
                    await _appDbContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception e)
                {
                    await trans?.RollbackAsync();
                }
            });
            return true;
        }

        public IEnumerable<CategoryDto> GetAll()
        {
            var categories = _appDbContext.Categories.ToList();
            IEnumerable<CategoryDto> result = _mapper.Map<List<Category>, IEnumerable<CategoryDto>>(categories);
            return result;
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _appDbContext.Categories.FindAsync(id);
            if (category is null)
            {
                throw new BadHttpRequestException(Exceptions.CategoryNotFound);
            }
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<bool> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto)
        {
            if (updateCategoryDto is null)
            {
                throw new BadHttpRequestException(Exceptions.BadRequest);
            }
            var category = _appDbContext.Categories.FirstOrDefault(x => x.Id == id);
            if (category is null)
            {
                return false;
            }
            var categoryDto = _mapper.Map(updateCategoryDto, category);
            var init = _appDbContext.Database.CreateExecutionStrategy();
            await init.ExecuteAsync(async () =>
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    _appDbContext.Categories.Update(categoryDto);
                    await _appDbContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception e)
                {
                    await trans?.RollbackAsync();
                }
            });
            return true;
        }
    }
}
