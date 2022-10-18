using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Categories;

namespace VegetableShop.Mvc.ApiClient.Categories
{
    public interface ICategoryApiClient
    {
        Task<IEnumerable<CategoryViewModel>> GetAllAsync();
        Task<CategoryViewModel> GetCategoryByIdAsync(int id);
        Task<Response> DeleteAsync(int id);
        Task<CreateResponse> CreateAsync(CreateCategoryRequest request);
        Task<Response> UpdateAsync(int id, UpdateCategoryRequest request);
    }
}
