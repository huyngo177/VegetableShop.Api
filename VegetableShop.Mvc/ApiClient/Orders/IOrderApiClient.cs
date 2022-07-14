using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Orders;

namespace VegetableShop.Mvc.ApiClient.Orders
{
    public interface IOrderApiClient
    {
        Task<IEnumerable<OrderViewModel>> GetAllAsync();
        Task<OrderViewModel> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderViewModel>> GetOrderByUserIdAsync(int id);
        Task<CreateResponse> CreateAsync();
        Task<Response> UpdateAsync(int id, UpdateOrderRequest request);
        Task<Response> DeleteAsync(int id);
    }
}
