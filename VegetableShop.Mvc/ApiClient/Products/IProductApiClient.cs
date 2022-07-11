using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Products;

namespace VegetableShop.Mvc.ApiClient.Products
{
    public interface IProductApiClient
    {
        Task<IEnumerable<ProductViewModel>> GetAllAsync();
        Task<CreateProductRequest> GetCategory();
        Task<ProductViewModel> GetProductByIdAsync(int id);
        Task<CreateResponse> CreateAsync(CreateProductRequest request);
        Task<Response> UpdateAsync(int id, UpdateProductRequest request);
        Task<Response> DeleteAsync(int id);
    }
}
