using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Api.Dto.Orders;
using VegetableShop.Api.Services.Orders;
using VegetableShop.Api.Services.Products;

namespace VegetableShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IConfiguration _configuration;
        public OrdersController(IOrderService orderService, IConfiguration configuration)
        {
            _orderService = orderService;
            _configuration = configuration;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderDto createOrderDto)
        {
            var order = await _orderService.CreateAsync(createOrderDto);
            if (order.IsSuccess)
            {
                return Created(new Uri($"{_configuration["BaseAddress"]}/api/products/{order.orderDto.Id}"), order.orderDto);
            }
            return BadRequest(order.Message);
        }
    }
}
