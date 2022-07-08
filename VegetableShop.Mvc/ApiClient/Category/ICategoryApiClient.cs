using VegetableShop.Mvc.Models.Category;

namespace VegetableShop.Mvc.ApiClient.Category
{
    public interface ICategoryApiClient
    {
        Task<IEnumerable<CategoryViewModel>> GetAllAsync();
    }
}
