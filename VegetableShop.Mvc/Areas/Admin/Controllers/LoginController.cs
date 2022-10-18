using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VegetableShop.Mvc.ApiClient.User;
using VegetableShop.Mvc.Models;

namespace VegetableShop.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public LoginController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginRequest request, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var result = await _userApiClient.Login(request);
            if (result.Token is null)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = true
            };
            var userPrincipal = ValidateToken(result.Token);

            HttpContext.Session.SetString("Token", result.Token);
            HttpContext.Response.Cookies.Append(
                "userId",
                result.userId.ToString(),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1)
                });
            await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        userPrincipal,
                        authProperties);

            if (userPrincipal.IsInRole("Admin"))
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Forbidden", "Home");
        }
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = _configuration["Jwt:Audience"];
            validationParameters.ValidIssuer = _configuration["Jwt:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }
    }
}
