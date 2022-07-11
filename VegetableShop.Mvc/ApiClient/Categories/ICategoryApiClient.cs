using VegetableShop.Api.Data.Entities;
using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Categories;
using VegetableShop.Mvc.Models.Categories;

namespace VegetableShop.Mvc.ApiClient.Categories
{
    public interface ICategoryApiClient
    {
        Task<IEnumerable<CategoryViewModel>> GetAllAsync();
        Task<IList<Category>> SelectAll();
        Task<CategoryViewModel> GetCategoryByIdAsync(int id);
        Task<Response> DeleteAsync(int id);
        Task<CreateResponse> CreateAsync(CreateCategoryRequest request);
        Task<Response> UpdateAsync(int id, UpdateCategoryRequest request);
    }
}
