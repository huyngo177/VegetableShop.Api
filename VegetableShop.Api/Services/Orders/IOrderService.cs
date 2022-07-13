using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Orders;

namespace VegetableShop.Api.Services.Orders
{
    public interface IOrderService
    {
        IEnumerable<OrderDto> GetAll();
        Task<OrderDto> GetOrderByIdAsync(int id);
        Task<CreateResponse> CreateAsync(CreateOrderDto createOrderDto);
        Task<bool> UpdateAsync(int id, UpdateOrderDto updateOrderDto);
        Task<bool> DeLeteAsync(int id);
        //Task<IEnumerable<OrderDto>> GetOrderByUserIdAsync(int userId);
    }
}
