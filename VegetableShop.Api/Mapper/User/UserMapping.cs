using AutoMapper;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto.User;

namespace VegetableShop.Api.Mapper.User
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<AppUser, AppUserDto>()
                .ForMember(x => x.UserName, y => y.MapFrom(x => x.UserName.ToUpper()));
            CreateMap<CreateUserDto, AppUser>();
            CreateMap<UpdateUserDto, AppUser>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => !string.IsNullOrEmpty(srcMember?.ToString())));
        }
    }
}
