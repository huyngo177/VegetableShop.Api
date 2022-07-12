using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Products;

namespace VegetableShop.Api.Services.Products
{
    public interface IProductService
    {
        IEnumerable<ProductDto> GetAll();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task<CreateResponse> CreateAsync(CreateProductDto createProductDto);
        Task<bool> UpdateAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> DeLeteAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductByCategoryIdAsync(int categoryId);
    }
}
