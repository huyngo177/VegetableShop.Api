using AutoMapper;
using VegetableShop.Mvc.Models.Products;

namespace VegetableShop.Mvc.Mapper.Product
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductViewModel, UpdateProductRequest>();
        }
    }
}
