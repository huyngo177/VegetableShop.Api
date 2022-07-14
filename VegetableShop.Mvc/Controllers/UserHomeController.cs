using Microsoft.AspNetCore.Mvc;
using VegetableShop.Api.Dto.Page;
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

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 4)
        {
            var request = new GetProductPageRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            ViewBag.Keyword = keyword;
            return View(await _productApiClient.GetAllAsync(request));
        }
        [HttpGet]
        public async Task<IActionResult> GetProductByCategoryId(int id)
        {
            var products = await _productApiClient.GetProductByCategoryIdAsync(id);
            return View("Index", products);
        }
    }
}
