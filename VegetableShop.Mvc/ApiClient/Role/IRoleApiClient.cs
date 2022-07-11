using VegetableShop.Api.Data.Entities;
using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Role;

namespace VegetableShop.Mvc.ApiClient.Role
{
    public interface IRoleApiClient
    {
        Task<IEnumerable<RoleViewModel>> GetAllAsync();
        Task<IList<AppRole>> SelectAll();
        Task<RoleViewModel> GetCategoryByIdAsync(int id);
        Task<Response> DeleteAsync(int id);
        Task<CreateResponse> CreateAsync(CreateRoleRequest request);
        Task<Response> UpdateAsync(int id, UpdateRoleRequest request);
    }
}
