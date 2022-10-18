using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Api.Dto.Page;
using VegetableShop.Mvc.ApiClient.Products;
using VegetableShop.Mvc.ApiClient.User;
using VegetableShop.Mvc.Models.User;

namespace VegetableShop.Mvc.Controllers
{
    [Authorize]
    public class UserHomeController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IUserApiClient _userApiClient;
        public UserHomeController(IProductApiClient productApiClient, IUserApiClient userApiClient)
        {
            _productApiClient = productApiClient;
            _userApiClient = userApiClient;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 4, string status = "Available")
        {
            var request = new GetProductPageRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId,
                Status = status
            };
            ViewBag.Keyword = keyword;
            return View(await _productApiClient.GetAllAsync(request));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByCategoryId(int id)
        {
            var products = await _productApiClient.GetProductByCategoryIdAsync(id);
            return View("Index", products);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            return View(await _userApiClient.GetUserByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string username)
        {
            var user = await _userApiClient.GetUserByNameAsync(username);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _userApiClient.GetUserByIdAsync(id);
            return View(new UpdateUserRequest()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateUserRequest request)
        {
            var response = await _userApiClient.UpdateAsync(id, request);
            if (response.IsSuccess)
            {
                var user = await _userApiClient.GetUserByIdAsync(id);
                return Json(new
                {
                    isValid = true,
                    html = Helper.RenderRazorViewToString(this, "_ProfilePartialView",
                    user)
                });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Update", request) });
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePassword(int id)
        {
            if (await _userApiClient.GetUserByIdAsync(id) is not null)
            {
                return View();
            }
            return View("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(int id, UpdatePasswordRequest request)
        {
            var response = await _userApiClient.UpdatePasswordAsync(id, request);
            if (response.IsSuccess)
            {
                var user = await _userApiClient.GetUserByIdAsync(id);
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ProfilePartialView", user) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "UpdatePassword", request) });
        }
    }
}
