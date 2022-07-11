using Microsoft.AspNetCore.Mvc;
using VegetableShop.Mvc.ApiClient.Products;
using VegetableShop.Mvc.Models.Products;

namespace VegetableShop.Mvc.Controllers
{
    public class ProductPageController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        public ProductPageController(IProductApiClient productApiClient)
        {
            _productApiClient = productApiClient;
        }

        public async Task<IActionResult> Detail(int id)
        {
            var product = await _productApiClient.GetProductByIdAsync(id);
            return View(new ProductDetailViewModel()
            {
                Product = product,
            });
        }

    }
}
