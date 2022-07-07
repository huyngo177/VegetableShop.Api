using AutoMapper;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto.Products;

namespace VegetableShop.Api.Mapper.Products
{
    public class ProductMapping : Profile
    {
        public ProductMapping()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(x => x.CategoryName, y => y.MapFrom(x => x.Category.Name));
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>()
                .ForAllMembers(opts => opts.Condition((source, dest, sourceMember) => sourceMember != null || sourceMember != ""));
        }
    }
}
