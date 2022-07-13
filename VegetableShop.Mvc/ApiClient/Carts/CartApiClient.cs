﻿using AutoMapper;
using Newtonsoft.Json;
using VegetableShop.Mvc.ApiClient.Products;
using VegetableShop.Mvc.Models.Sale;

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

        public CartApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IMapper mapper, IProductApiClient productApiClient, IHttpContextAccessor context)
            : base(configuration, httpClientFactory, mapper)
        {
            _configuration = configuration;
            _clientFactory = httpClientFactory;
            _mapper = mapper;
            _imagePath = $"{_configuration["BaseAddress"]}";
            _productApiClient = productApiClient;
            this.context = context;
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

        public async Task<bool> Checkout(CheckoutViewModel checkoutViewModel)
        {
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
            return true;
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

