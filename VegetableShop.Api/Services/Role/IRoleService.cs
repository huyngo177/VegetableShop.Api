using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Role;

namespace VegetableShop.Api.Services.Role
{
    public interface IRoleService
    {
        IEnumerable<AppRoleDto> GetAll();
        Task<AppRoleDto> GetRoleByIdAsync(int id);
        Task<CreateResponse> CreateAsync(CreateRoleDto createRoleDto);
        Task<bool> UpdateAsync(int id, UpdateRoleDto updateRoleDto);
        Task<bool> DeleteAsync(int id);
    }
}
