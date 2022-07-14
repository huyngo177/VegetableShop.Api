using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Mvc.ApiClient.Orders;
using VegetableShop.Mvc.Models.Orders;

namespace VegetableShop.Mvc.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderApiClient _orderApiClient;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public OrderController(IOrderApiClient orderApiClient, IMapper mapper, IConfiguration configuration)
        {
            _orderApiClient = orderApiClient;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _orderApiClient.GetAllAsync());
        }

        [HttpGet]
        public IActionResult CheckOut()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> CheckOut(CreateOrderRequest request)
        //{
        //    var order = await _orderApiClient.CreateAsync(request);
        //    if (order.IsSuccess)
        //    {
        //        return RedirectToAction("Index", "UserHome");
        //    }
        //    return View();
        //}
    }
}
