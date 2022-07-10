using VegetableShop.Api.Data.Entities;
using VegetableShop.Mvc.Models.Categories;

namespace VegetableShop.Mvc.ApiClient.Categories
{
    public interface ICategoryApiClient
    {
        Task<IEnumerable<CategoryViewModel>> GetAllAsync();
        Task<IList<Category>> SelectAll();
    }
}
