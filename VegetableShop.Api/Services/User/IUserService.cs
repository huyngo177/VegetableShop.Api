using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Page;
using VegetableShop.Api.Dto.User;

namespace VegetableShop.Api.Services.User
{
    public interface IUserService
    {
        Task<PageResult<AppUserDto>> GetAsync(GetUserPageRequest request);
        Task<AppUserDto> GetUserByIdAsync(int id);
        Task<AppUserDto> GetUserByNameAsync(string username);
        Task<CreateResponse> CreateAsync(CreateUserDto createUserDto);
        Task<bool> UpdateAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeLeteAsync(int id);
        Task<bool> ChangeLockedStatusAsync(int id);
        Task<bool> AssignRoleAsync(int id, IEnumerable<string> roles);
        Task<bool> RemoveRoleAsync(int id, IEnumerable<string> roles);
        Task<Response> LoginAsync(LoginDto loginDto);
        Task<TokenResult> RefreshTokenAsync(TokenDto tokenDto);
        Task<bool> RevokeAsync(string username);
        Task RevokeAllAsync();
    }
}
