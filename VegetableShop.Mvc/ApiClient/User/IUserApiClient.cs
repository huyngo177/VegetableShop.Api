using VegetableShop.Api.Dto.Page;
using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Page;
using VegetableShop.Mvc.Models.User;

namespace VegetableShop.Mvc.ApiClient.User
{
    public interface IUserApiClient
    {
        Task<Response> Login(LoginRequest request);
        Task<PaginationResult<UserViewModel>> GetAllAsync(PageViewModel request);
        Task<UserViewModel> GetUserByIdAsync(int id);
        Task<UserViewModel> GetUserByNameAsync(string username);
        Task<CreateResponse> CreateAsync(CreateUserRequest request);
        Task<Response> UpdateAsync(int id, UpdateUserRequest request);
        Task<Response> UpdatePasswordAsync(int id, UpdatePasswordRequest request);
        Task<Response> LockAsync(int id);
        Task<Response> DeleteAsync(int id);
        Task<Response> RestoreLockedStatusAsync(int id);
        Task<IEnumerable<string>> GetRolesByUserIdAsync(int id);
        Task<Response> AssignRoleAsync(int id, string role);
        Task<Response> RemoveRoleAsync(int id, string role);
        //Task<TokenResult> RefreshTokenAsync(TokenDto tokenDto);
        Task<Response> RevokeByUsernameAsync(string username);
        Task RevokeAsync();
    }
}
