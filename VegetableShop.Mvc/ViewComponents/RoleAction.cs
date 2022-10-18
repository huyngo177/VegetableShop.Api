using Microsoft.AspNetCore.Mvc;
using VegetableShop.Mvc.ApiClient.User;

namespace VegetableShop.Mvc.ViewComponents
{
    public class RoleAction : ViewComponent
    {
        private readonly IUserApiClient _userApiClient;
        public RoleAction(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var roles = await _userApiClient.GetRolesByUserIdAsync(id);
            return View(roles);
        }
    }
}
