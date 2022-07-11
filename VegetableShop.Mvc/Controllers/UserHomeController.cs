using Microsoft.AspNetCore.Mvc;
using VegetableShop.Mvc.ApiClient.Products;

namespace VegetableShop.Mvc.Controllers
{
    public class UserHomeController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        public UserHomeController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productApiClient.GetAllAsync());
        }
    }
}
