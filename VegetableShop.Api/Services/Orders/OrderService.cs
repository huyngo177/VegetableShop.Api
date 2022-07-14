using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using VegetableShop.Api.Common;
using VegetableShop.Api.Data.EF;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Api.Dto;
using VegetableShop.Api.Dto.Orders;
using VegetableShop.Api.Services.Products;

namespace VegetableShop.Api.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public OrderService(AppDbContext appDbContext, IConfiguration configuration, IMapper mapper, IProductService productService)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
            _mapper = mapper;
            _productService = productService;
        }

        public CreateResponse CreateAsync(CreateOrderDto createOrderDto)
        {
            if (createOrderDto is null)
            {
                throw new AppException(Exceptions.BadRequest);
            }

            var user = _appDbContext.AppUsers.FirstOrDefault(x => x.Id == createOrderDto.UserId);
            if (user is null)
            {
                throw new KeyNotFoundException(Exceptions.UserNotFound);
            }
            _mapper.Map(createOrderDto, user);
            var orderDto = new OrderDto();
            var orders = new List<Order>();
            var order = new Order();
            var orderDetail = new List<OrderDetail>();
            var productCart = new Product();

            foreach (var item in createOrderDto.Items)
            {
                var product = _appDbContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
                if (product is null)
                {
                    continue;
                }
                var pro = new OrderDetailDto()
                {
                    SupTotal = item.Quantity * item.Price,
                    Quantity = item.Quantity
                };
                orderDetail.Add(_mapper.Map<OrderDetail>(pro));
            }
            decimal totalPrice = 0;
            foreach (var item in orderDetail)
            {
                totalPrice += item.SupTotal;
            }
            order.TotalPrice = totalPrice;
            order.OrderDetails = orderDetail;

            var init = _appDbContext.Database.CreateExecutionStrategy();
            init.Execute(() =>
            {
                using var trans = _appDbContext.Database.BeginTransaction();
                try
                {
                    orders.Add(order);
                    user.Orders = orders;
                    _appDbContext.AppUsers.Update(user);
                    _appDbContext.Orders.Add(order);
                    foreach (var item in createOrderDto.Items)
                    {
                        var product = _appDbContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
                        if (product is null)
                        {
                            continue;
                        }
                        product.Stock -= item.Quantity;
                        product.OrderDetail = orderDetail;
                        _appDbContext.Products.Update(product);
                    }
                    _appDbContext.SaveChanges();
                    _mapper.Map(order, orderDto);
                    _mapper.Map(user, orderDto);
                    orderDto.FullName = $"{user.FirstName} {user.LastName}";
                    orderDto.OrderDetails = orderDetail;
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans?.Rollback();
                }
            });
            return new CreateResponse(orderDto, Messages.CreateSuccess);
        }

        public async Task<bool> DeLeteAsync(int id)
        {
            var order = _appDbContext.Orders.FirstOrDefault(x => x.Id == id);
            if (order is null)
            {
                throw new KeyNotFoundException(Exceptions.OrderNotFound);
            }
            var init = _appDbContext.Database.CreateExecutionStrategy();
            await init.ExecuteAsync(async () =>
            {
                using var trans = await _appDbContext.Database.BeginTransactionAsync();
                try
                {
                    _appDbContext.Orders.Remove(order);
                    await _appDbContext.SaveChangesAsync();
                    await trans.CommitAsync();
                }
                catch (Exception e)
                {
                    await trans?.RollbackAsync();
                }
            });
            return true;
        }

        public async Task<IEnumerable<OrderDto>> GetAll()
        {
            var orders = await _appDbContext.Orders.ToListAsync();
            var orderList = new List<OrderDto>();
            var orderDto = new OrderDto();
            foreach (var order in orders)
            {
                var user = await _appDbContext.AppUsers.FirstOrDefaultAsync(x => x.Id == order.AppUserId);
                if (user is null)
                {
                    throw new KeyNotFoundException(Exceptions.UserNotFound);
                }
                _mapper.Map(user, orderDto);
                orderList.Add(orderDto);
            }
            IEnumerable<OrderDto> result = _mapper.Map<IList<Order>, IEnumerable<OrderDto>>(orders, orderList);
            return result;
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            var order = await _appDbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (order is null)
            {
                throw new KeyNotFoundException(Exceptions.OrderNotFound);
            }

            var orderDto = new OrderDto();

            var user = await _appDbContext.AppUsers.FirstOrDefaultAsync(x => x.Id == order.AppUserId);
            if (user is null)
            {
                throw new KeyNotFoundException(Exceptions.UserNotFound);
            }
            _mapper.Map(user, orderDto);
            _mapper.Map(order, orderDto);
            return orderDto;
        }

        public async Task<IEnumerable<OrderDto>> GetOrderByUserIdAsync(int userId)
        {
            var orders = await _appDbContext.Orders.ToListAsync();
            var orderList = new List<Order>();
            foreach (var order in orders)
            {
                if (order.AppUserId != userId)
                {
                    throw new KeyNotFoundException(Exceptions.UserNotFound);
                }
                orderList.Add(order);
            }
            var orderDtoList = new List<OrderDto>();
            var orderDto = new OrderDto();
            foreach (var order in orderList)
            {
                var user = await _appDbContext.AppUsers.FirstOrDefaultAsync(x => x.Id == order.AppUserId);
                if (user is null)
                {
                    throw new KeyNotFoundException(Exceptions.UserNotFound);
                }
                _mapper.Map(user, orderDto);
                orderDtoList.Add(orderDto);
            }
            IEnumerable<OrderDto> result = _mapper.Map<IList<Order>, IEnumerable<OrderDto>>(orders, orderDtoList);
            return result;
        }

        public async Task<bool> UpdateAsync(int id, UpdateOrderDto updateOrderDto)
        {
            var result = await _appDbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (result is null)
            {
                throw new KeyNotFoundException(Exceptions.OrderNotFound);
            }
            var user = await _appDbContext.AppUsers.FirstOrDefaultAsync(x => x.Id == result.AppUserId);
            _mapper.Map(updateOrderDto, user);
            var orderDto = new OrderDto();
            var orders = new List<Order>();
            var order = new Order();
            var orderDetail = new List<OrderDetail>();
            var productCart = new Product();

            foreach (var item in updateOrderDto.Items)
            {
                var product = _appDbContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
                if (product is null)
                {
                    continue;
                }
                var pro = new OrderDetailDto()
                {
                    SupTotal = item.Quantity * item.Price,
                    Quantity = item.Quantity
                };
                orderDetail.Add(_mapper.Map<OrderDetail>(pro));
            }
            decimal totalPrice = 0;
            foreach (var item in orderDetail)
            {
                totalPrice += item.SupTotal;
            }
            order.TotalPrice = totalPrice;
            order.OrderDetails = orderDetail;

            var init = _appDbContext.Database.CreateExecutionStrategy();
            init.Execute(() =>
            {
                using var trans = _appDbContext.Database.BeginTransaction();
                try
                {
                    orders.Add(order);
                    user.Orders = orders;
                    _appDbContext.AppUsers.Update(user);
                    _appDbContext.Orders.Update(order);
                    foreach (var item in updateOrderDto.Items)
                    {
                        var product = _appDbContext.Products.FirstOrDefault(x => x.Id == item.ProductId);
                        if (product is null)
                        {
                            continue;
                        }
                        foreach (var record in result.OrderDetails)
                        {
                            product.Stock = product.Stock + record.Quantity - item.Quantity;
                        }
                        if (product.Stock == 0)
                        {
                            product.Status = "Unavailable";
                        }
                        product.OrderDetail = orderDetail;
                        _appDbContext.Products.Update(product);
                    }
                    _appDbContext.SaveChanges();
                    _mapper.Map(order, orderDto);
                    _mapper.Map(user, orderDto);
                    orderDto.FullName = $"{user.FirstName} {user.LastName}";
                    orderDto.OrderDetails = orderDetail;
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans?.Rollback();
                }
            });
            return true;
        }
    }
}
