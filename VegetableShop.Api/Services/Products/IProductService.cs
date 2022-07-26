using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Page;
using VegetableShop.Api.Dto.Products;

namespace VegetableShop.Api.Services.Products
{
    public interface IProductService
    {
        Task<PageResult<ProductDto>> GetAllAsync(GetProductPageRequest request);
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<CreateResponse> CreateAsync(CreateProductDto createProductDto);
        Task<bool> UpdateAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeLeteAsync(int id);
        Task<bool> UpdateStock(int id, int quantity);
        Task<bool> ChangeStatusProduct(int id);
    }
}
