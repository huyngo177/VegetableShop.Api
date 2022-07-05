using AutoMapper;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto.Category;

namespace VegetableShop.Api.Mapper.Categories
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<CategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>()
                .ForAllMembers(opts => opts.Condition((source, dest, sourceMember) => sourceMember != null || sourceMember != ""));
            CreateMap<UpdateCategoryDto, CategoryDto>();
        }
    }
}
