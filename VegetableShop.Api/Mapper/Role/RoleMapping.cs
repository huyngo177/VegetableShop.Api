using AutoMapper;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto.Role;

namespace VegetableShop.Api.Mapper.Role
{
    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<CreateRoleDto, AppRole>();
            CreateMap<AppRole, AppRoleDto>();
            CreateMap<UpdateRoleDto, AppRole>();
        }
    }
}
