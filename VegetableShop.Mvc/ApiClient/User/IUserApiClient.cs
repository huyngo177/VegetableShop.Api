using VegetableShop.Api.Dto.Page;
using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.User;

namespace VegetableShop.Mvc.ApiClient.User
{
    public interface IUserApiClient
    {
        Task<Response> Login(LoginRequest request);
        Task<PageResult<UserViewModel>> GetAllAsync(GetUserPageRequest request);
        Task<UserViewModel> GetUserByIdAsync(int id);
        Task<CreateResponse> CreateAsync(CreateUserRequest request);
        Task<Response> UpdateAsync(int id, UpdateUserRequest request);
        Task<Response> DeleteAsync(int id);
        Task RevokeAsync();
    }
}
