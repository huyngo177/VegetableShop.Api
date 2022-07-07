using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.User;

namespace VegetableShop.Mvc.ApiClient.User
{
    public interface IUserApiClient
    {
        Task<Response> Login(LoginRequest request);
        Task<IEnumerable<UserViewModel>> GetAllAsync();
        Task<UserViewModel> GetUserByIdAsync(int id);
        Task<CreateResponse> CreateAsync(CreateUserRequest request);
        Task<Response> UpdateAsync(int id, UpdateUserRequest request);
        Task<Response> DeleteAsync(int id);
    }
}
