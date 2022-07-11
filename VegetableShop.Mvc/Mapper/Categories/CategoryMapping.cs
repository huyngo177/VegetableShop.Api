using AutoMapper;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Mvc.Models.Categories;

namespace VegetableShop.Mvc.Mapper.Categories
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<UpdateCategoryRequest, CategoryViewModel>();
        }
    }
}
