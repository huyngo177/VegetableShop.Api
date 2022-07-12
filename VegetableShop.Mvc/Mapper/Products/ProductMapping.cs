using AutoMapper;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Mvc.Models.Products;

namespace VegetableShop.Mvc.Mapper.Products
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<ProductViewModel, UpdateProductRequest>();
            CreateMap<ProductViewModel, Product>();
        }
    }
}
