using AutoMapper;
using Newtonsoft.Json;
using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Orders;
using VegetableShop.Mvc.Models.Sale;

namespace VegetableShop.Mvc.ApiClient.Orders
{
    public class OrdeApiClient : BaseApiClient, IOrderApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string _imagePath;
        private readonly IOrderApiClient _productApiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _client;

        public OrdeApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IMapper mapper, IOrderApiClient orderApiClient, IHttpContextAccessor httpContextAccessor)
        : base(configuration, httpClientFactory, mapper, httpContextAccessor)
        {
            _configuration = configuration;
            _clientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _client = _clientFactory.CreateClient();
            _client.BaseAddress = new Uri($"{_configuration["BaseAddress"]}");
        }
        public async Task<CreateResponse> CreateAsync()
        {
            var session = _httpContextAccessor.HttpContext.Session.GetString("CartSession");
            List<CartItemViewModel> currentCart = new List<CartItemViewModel>();
            if (session != null)
            {
                currentCart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(session);
            }
            var request = new CreateOrderRequest()
            {

            };
            var response = await _client.PostAsync("api/orders", HandleRequest(request));
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CreateResponse>(data);
            }
            var body = JsonConvert.DeserializeObject<CreateResponse>(await response.Content.ReadAsStringAsync());
            body.IsSuccess = false;
            return body;
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"api/orders/{id}");
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<IEnumerable<OrderViewModel>> GetAllAsync()
        {
            var orders = await GetAsync<IEnumerable<OrderViewModel>>("api/orders");
            return orders;
        }

        public async Task<OrderViewModel> GetOrderByIdAsync(int id)
        {
            var order = await GetAsync<OrderViewModel>($"api/orders/{id}");
            return order;
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrderByUserIdAsync(int id)
        {
            var order = await GetAsync<IEnumerable<OrderViewModel>>($"api/orders/userId/{id}");
            return order;
        }

        public async Task<Response> UpdateAsync(int id, UpdateOrderRequest request)
        {
            var response = await _client.PutAsync($"api/orders/{id}", HandleRequest(request));
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            };
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }
    }
}
