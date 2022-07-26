using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Orders;

namespace VegetableShop.Api.Services.Orders
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAll();
        Task<OrderDto> GetOrderByIdAsync(int id);
        CreateResponse CreateAsync(CreateOrderDto createOrderDto);
        Task<bool> DeLeteAsync(int id);
        Task<IEnumerable<OrderDto>> GetOrderByUserIdAsync(int userId);
    }
}
