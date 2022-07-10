using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Mvc.ApiClient.User;
using VegetableShop.Mvc.Models.User;

namespace VegetableShop.Mvc.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IMapper _mapper;
        public UserController(IUserApiClient userApiClient, IMapper mapper)
        {
            _userApiClient = userApiClient;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userApiClient.GetAllAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var user = await _userApiClient.GetUserByIdAsync(id);
            return View(user);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            var response = await _userApiClient.CreateAsync(request);
            if (response.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return View(request);
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
                return RedirectToAction("Detail", new { id = id });
            }
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userApiClient.DeleteAsync(id);
            if (response.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return View("Index", new { message = response.Message });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //if (HttpContext.Request.Cookies.Count > 0)
            //{
            //    var siteCookies = HttpContext.Request.Cookies.Where(c => c.Key.Contains(".AspNetCore.") || c.Key.Contains("Microsoft.Authentication"));
            //    foreach (var cookie in siteCookies)
            //    {
            //        Response.Cookies.Delete(cookie.Key);
            //    }
            //}
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme) ;
            //localStorage.clear();
            //HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
