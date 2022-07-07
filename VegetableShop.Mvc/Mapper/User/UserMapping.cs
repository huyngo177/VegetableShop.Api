using AutoMapper;
using VegetableShop.Api.Dto.User;
using VegetableShop.Mvc.Models.User;

namespace VegetableShop.Mvc.Mapper.User
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserViewModel, UpdateUserRequest>();

            CreateMap<AppUserDto, UserViewModel>();
        }
    }
}
