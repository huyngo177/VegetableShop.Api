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

        public async Task<CreateResponse> CreateAsync(CreateOrderDto createOrderDto)
        {
            if (createOrderDto is null)
            {
                throw new AppException(Exceptions.BadRequest);
            }

            var user = _appDbContext.AppUsers.FirstOrDefault(x => x.UserName == createOrderDto.UserName);
            if (user is null)
            {
                throw new KeyNotFoundException(Exceptions.UserNotFound);
            }
            var userDto = _mapper.Map<AppUser>(createOrderDto);
            var order = new List<Order>();
            var orderDetail = new List<OrderDetail>();
            var productCart = new Product();
            
            foreach (var item in createOrderDto.Items)
            {
                var product = _appDbContext.Products.FirstOrDefault(x => x.Name == item.Name);
                if (product is null)
                {
                    continue;
                }
                var pro = new Product()
                {
                    Name = product.Name,
                    Price = item.Quantity * item.Price
                };
                productList.Add(pro);
            }
            decimal totalPrice = 0;
            foreach (var item in productList)
            {
                totalPrice += item.Price;
            }
            orderDetail.Products = productList;
            orderDetail.TotalPrice = totalPrice;

            var init = _appDbContext.Database.CreateExecutionStrategy();
            init.Execute(() =>
            {
                using var trans = _appDbContext.Database.BeginTransaction();
                try
                {
                    
                    
                    _appDbContext.Orders.Add(order);
                    foreach (var item in createOrderDto.Items)
                    {
                        var product = _appDbContext.Products.FirstOrDefault(x => x.Name == item.Name);
                        if (product is null)
                        {
                            continue;
                        }
                        product.Stock -= item.Quantity;
                        _appDbContext.Products.Update(product);
                    }
                    _appDbContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans?.Rollback();
                }
            });
            var orderDto = new OrderDto()
            {
                UserName = userDto.UserName,
                FullName = $"{userDto.FirstName} {userDto.LastName}",
                PhoneNumber = userDto.PhoneNumber,
                Address=userDto.Address,
                OrderDate=createOrderDto.OrderDate,
                OrderDetails = { TotalPrice = totalPrice,OrderId= },
            };
            orderDto.
            orderDto.OrderDetails = orderDetail;
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

        public IEnumerable<OrderDto> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDto> GetOrderByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        //public async Task<IEnumerable<OrderDto>> GetOrderByUserIdAsync(int userId)
        //{
        //    var user = await _appDbContext.AppUsers.FirstOrDefaultAsync(x => x.Id == userId);
        //    if (user is null)
        //    {
        //        throw new KeyNotFoundException(Exceptions.UserNotFound);
        //    }
        //}

        public async Task<bool> UpdateAsync(int id, UpdateOrderDto updateOrderDto)
        {
            throw new NotImplementedException();
        }
    }
}
