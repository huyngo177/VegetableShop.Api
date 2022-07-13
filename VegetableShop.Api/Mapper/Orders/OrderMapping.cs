using AutoMapper;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto.Orders;

namespace VegetableShop.Api.Mapper.Orders
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<CreateOrderDto, Order>();
            CreateMap<UpdateOrderDto, Order>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<CreateOrderDto, AppUser>();
            CreateMap<CreateOrderDto, Product>();
            CreateMap<CreateOrderDto, OrderDto>();
        }
    }
}
