using VegetableShop.Api.Dto.Page;
using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Products;

namespace VegetableShop.Mvc.ApiClient.Products
{
    public interface IProductApiClient
    {
        Task<PageResult<ProductViewModel>> GetAllAsync(GetProductPageRequest request);
        Task<ProductViewModel> GetProductByIdAsync(int id);
        Task<CreateResponse> CreateAsync(CreateProductRequest request);
        Task<Response> UpdateAsync(int id, UpdateProductRequest request);
        Task<Response> DeleteAsync(int id);
        Task<IEnumerable<ProductViewModel>> GetProductByCategoryIdAsync(int categoryId);
    }
}
