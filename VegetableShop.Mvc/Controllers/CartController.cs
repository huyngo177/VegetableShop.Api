using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VegetableShop.Mvc.ApiClient.Carts;
using VegetableShop.Mvc.ApiClient.Products;
using VegetableShop.Mvc.Models.Sale;

namespace VegetableShop.Mvc.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICartApiClient _cartApiClient;
        public CartController(IProductApiClient productApiClient, ICartApiClient cartApiClient)
        {
            _productApiClient = productApiClient;
            _cartApiClient = cartApiClient;
        }

        public IActionResult Index()
        {
            var item = _cartApiClient.GetListItem();
            var temp = item.Count();
            if (temp == 0)
            {
                return RedirectToAction("AddToCart");
            }
            return View(new CheckoutViewModel()
            {
                CartItems = item
            });
        }

        public async Task<IActionResult> AddToCart(int id, int quantity)
        {
            var item = await _cartApiClient.AddToCart(id, quantity);
            return RedirectToAction("Index",new CheckoutViewModel()
            {
                CartItems = item
            });
        }

        [HttpGet]
        public IActionResult CheckOut()
        {
            return View(_cartApiClient.GetCheckoutViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CheckOut(CheckoutViewModel checkoutViewModel)
        {
            return RedirectToAction("Index", "UserHome");
        }

        public IActionResult UpdateCart(int id, int quantity)
        {
            var cart = _cartApiClient.UpdateCart(id, quantity);
            return View("Index");
        }
    }
}
