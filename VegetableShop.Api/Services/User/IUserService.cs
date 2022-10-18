using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Page;
using VegetableShop.Api.Dto.User;

namespace VegetableShop.Api.Services.User
{
    public interface IUserService
    {
        Task<PaginationResult<AppUserDto>> GetAsync(PageDto pageDto);
        Task<AppUserDto> GetUserByIdAsync(int id);
        Task<AppUserDto> GetUserByNameAsync(string username);
        Task<CreateResponse> CreateAsync(CreateUserDto createUserDto);
        Task<bool> UpdateAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> UpdatePasswordAsync(int id, UpdatePasswordDto updatePasswordDto);
        Task<bool> DeLeteAsync(int id);
        Task<bool> ChangeLockedStatusAsync(int id, bool isLocked);
        Task<IEnumerable<string>> GetRolesByUserIdAsync(int id);
        Task<bool> AssignRoleAsync(int id, string role);
        Task<bool> RemoveRoleAsync(int id, string role);
        Task<Response> LoginAsync(LoginDto loginDto);
        Task<TokenResult> RefreshTokenAsync(TokenDto tokenDto);
        Task<bool> RevokeAsync(string username);
        Task RevokeAllAsync();
    }
}
