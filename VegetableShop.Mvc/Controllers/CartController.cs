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
            return View(new CheckoutViewModel()
            {
                CartItems = item
            });
        }

        public async Task<IActionResult> AddToCart(int id, int quantity = 1)
        {
            var item = await _cartApiClient.AddToCart(id, quantity);
            return RedirectToAction("Index","Cart", new CheckoutViewModel()
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
        public async Task<IActionResult> CheckOut(UserInfoRequest userInfoRequest)
        {
            var result = await _cartApiClient.Checkout(userInfoRequest);
            if(result.IsSuccess)
            {
                return RedirectToAction("Index", "UserHome");
            }
            return View(_cartApiClient.GetCheckoutViewModel());
        }

        public IActionResult UpdateCart(int id, int quantity)
        {
            var cart = _cartApiClient.UpdateCart(id, quantity);
            return View("Index");
        }

        public IActionResult RemoveItem(int id)
        {
            var cart = _cartApiClient.RemoveItemInCart(id);
            var count = cart.Count();
            if (count != 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "UserHome");
        }

        public IActionResult RemoveCart()
        {
            _cartApiClient.RemoveCart();
            return RedirectToAction("Index", "UserHome");
        }
    }
}
