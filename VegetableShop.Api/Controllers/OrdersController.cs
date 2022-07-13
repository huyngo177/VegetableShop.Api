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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orderService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order is null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpGet("userId/{id}")]
        public async Task<IActionResult> GetOrderByUserId(int id)
        {
            var order = await _orderService.GetOrderByUserIdAsync(id);
            if (order is null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromBody] UpdateOrderDto updateOrderDto, int id)
        {
            if (await _orderService.GetOrderByIdAsync(id) is null)
            {
                return NotFound();
            }
            var order = await _orderService.UpdateAsync(id, updateOrderDto);
            if (order)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _orderService.GetOrderByIdAsync(id) is null)
            {
                return NotFound();
            }
            var order = await _orderService.DeLeteAsync(id);
            if (order)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
