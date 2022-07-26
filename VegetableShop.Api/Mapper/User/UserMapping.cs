using AutoMapper;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto.User;

namespace VegetableShop.Api.Mapper.User
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<AppUser, AppUserDto>();
            CreateMap<CreateUserDto, AppUser>();
            CreateMap<UpdateUserDto, AppUser>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => !string.IsNullOrEmpty(srcMember?.ToString())));
        }
    }
}
