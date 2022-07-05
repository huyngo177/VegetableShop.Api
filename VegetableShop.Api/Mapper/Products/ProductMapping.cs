using AutoMapper;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto.Products;

namespace VegetableShop.Api.Mapper.Products
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<CreateProductDto, Category>()
                .ForMember(x => x.Name, y => y.MapFrom(x => x.CategoryName));
            CreateMap<UpdateProductDto, Product>()
                .ForMember(x => x.Name, y => y.MapFrom(x => x.CategoryName))
                .ForAllMembers(opts => opts.Condition((source, dest, sourceMember) => sourceMember != null || sourceMember != ""));
        }
    }
}
