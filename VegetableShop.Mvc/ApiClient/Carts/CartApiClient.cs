using AutoMapper;
using Newtonsoft.Json;
using VegetableShop.Mvc.Models;
using VegetableShop.Api.Dto.Orders;
using VegetableShop.Mvc.ApiClient.Products;
using VegetableShop.Mvc.Models.Sale;
using VegetableShop.Mvc.Models.User;

namespace VegetableShop.Mvc.ApiClient.Carts
{
    public class CartApiClient : BaseApiClient, ICartApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string _imagePath;
        private readonly IProductApiClient _productApiClient;
        private readonly IHttpContextAccessor context;
        private readonly HttpClient _client;

        public CartApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IMapper mapper, IProductApiClient productApiClient, IHttpContextAccessor context)
            : base(configuration, httpClientFactory, mapper)
        {
            _configuration = configuration;
            _clientFactory = httpClientFactory;
            _mapper = mapper;
            _imagePath = $"{_configuration["BaseAddress"]}";
            _productApiClient = productApiClient;
            this.context = context;
            _client = _clientFactory.CreateClient();
            _client.BaseAddress = new Uri($"{_configuration["BaseAddress"]}");
        }

        public List<CartItemViewModel> GetListItem()
        {
            var session = context.HttpContext.Session.GetString("CartSession");
            List<CartItemViewModel> carts = new List<CartItemViewModel>();
            if (!string.IsNullOrEmpty(session))
            {
                carts = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            return carts;
        }

        public async Task<List<CartItemViewModel>> AddToCart(int id, int quantity)
        {
            var session = context.HttpContext.Session.GetString("CartSession");
            var product = await _productApiClient.GetProductByIdAsync(id);
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (!string.IsNullOrEmpty(session))
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            if (currentCart.Any(x => x.ProductId == id))
            {
                quantity = currentCart.First(x => x.ProductId == id).Quantity + quantity;
            }

            var cartItem = new CartItemViewModel()
            {
                ProductId = id,
                Description = product.Description,
                Image = product.ImagePath,
                Name = product.Name,
                Price = product.Price,
                Quantity = quantity,
                Stock = product.Stock,
            };

            currentCart.Add(cartItem);

            context.HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(currentCart));
            return currentCart;
        }

        public IEnumerable<CartItemViewModel> UpdateCart(int id, int quantity)
        {
            var session = context.HttpContext.Session.GetString("CartSession");

            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (!string.IsNullOrEmpty(session))
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            foreach (var item in currentCart)
            {
                if (item.ProductId == id)
                {
                    if (quantity == 0)
                    {
                        currentCart.Remove(item);
                        break;
                    }
                    item.Quantity = quantity;
                }
            }
            context.HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(currentCart));
            return currentCart;
        }

        public async Task<CreateResponse> Checkout(UserInfoRequest userInfoRequest)
        {
            //_ = int.TryParse(context.HttpContext.Request.Cookies["userId"], out int id);
            int id = 1;
            var user = await GetAsync<UserViewModel>($"api/users/{id}");

            var currentItem = GetCheckoutViewModel();
            var orderDetail = new List<OrderDetailViewModel>();
            foreach (var item in currentItem.CartItems)
            {
                orderDetail.Add(new OrderDetailViewModel()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                });
            }
            if (user.Id == 0)
            {
                return new CreateResponse(false);
            }
            currentItem.CheckOutRequest.OrderDetails = orderDetail;
            currentItem.CheckOutRequest.PhoneNumber = user.PhoneNumber;
            currentItem.CheckOutRequest.Address = user.Address;
            currentItem.CheckOutRequest.FirstName = user.FirstName;
            currentItem.CheckOutRequest.LastName = user.LastName;
            var createOrderDto = new CreateOrderDto()
            {
                FirstName = currentItem.CheckOutRequest.FirstName,
                LastName = currentItem.CheckOutRequest.LastName,
                Address = currentItem.CheckOutRequest.Address,
                PhoneNumber = currentItem.CheckOutRequest.PhoneNumber,
                UserId = id,
            };

            //foreach (var item in currentItem.CartItems)
            //{
                
            //}
            var response = await _client.PostAsync("api/orders", HandleRequest(createOrderDto));
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CreateResponse>(data);
            }
            var body = JsonConvert.DeserializeObject<CreateResponse>(await response.Content.ReadAsStringAsync());
            body.IsSuccess = false;
            return body;
        }

        public CheckoutViewModel GetCheckoutViewModel()
        {
            var session = context.HttpContext.Session.GetString("CartSession");
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            var checkoutVm = new CheckoutViewModel()
            {
                CartItems = currentCart,
                CheckOutRequest = new CheckOutRequest()
            };
            return checkoutVm;
        }

        public void RemoveCart()
        {
            context.HttpContext.Session.Remove("CartSession");
        }
        public List<CartItemViewModel> RemoveItemInCart(int id)
        {
            var session = context.HttpContext.Session.GetString("CartSession");
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            foreach (var item in currentCart)
            {
                if (item.ProductId == id)
                {
                    currentCart.Remove(item);
                    break;
                }
            }
            context.HttpContext.Session.SetString("CartSession", JsonConvert.SerializeObject(currentCart));
            return currentCart;
        }
    }
}

