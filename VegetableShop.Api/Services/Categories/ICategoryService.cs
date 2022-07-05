using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Category;

namespace VegetableShop.Api.Services.Categories
{
    public interface ICategoryService
    {
        IEnumerable<CategoryDto> GetAll();
        Task<CategoryDto> GetCategoryByIdAsync(int id);
        Task<CreateResponse> CreateAsync(CreateCategoryDto createCategoryDto);
        Task<bool> UpdateAsync(int id, UpdateCategoryDto updateCategoryDto);
        Task<bool> DeleteAsync(int id);
    }
}
