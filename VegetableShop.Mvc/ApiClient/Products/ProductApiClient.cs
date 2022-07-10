using AutoMapper;
using Newtonsoft.Json;
using VegetableShop.Mvc.Models.Products;
using VegetableShop.Mvc.Models;

namespace VegetableShop.Mvc.ApiClient.Products
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        private readonly string _imagePath;
        public ProductApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IMapper mapper)
            : base(configuration, httpClientFactory, mapper)
        {
            _configuration = configuration;
            _clientFactory = httpClientFactory;
            _mapper = mapper;
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri($"{_configuration["BaseAddress"]}");
            _imagePath = $"{_configuration["BaseAddress"]}";
        }

        public async Task<CreateResponse> CreateAsync(CreateProductRequest request)
        {
            var response = await _client.PostAsync("api/products", HandleRequest(request));
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
            var response = await _client.DeleteAsync($"api/products/{id}");
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<IEnumerable<ProductViewModel>> GetAllAsync()
        {
            var products = await GetAsync<IEnumerable<ProductViewModel>>("api/products");
            var list = new List<ProductViewModel>();
            foreach (var product in products)
            {
                product.ImagePath = $"{_imagePath}{product.ImagePath}";
                list.Add(product);
            }
            return list;
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            var response = await GetAsync<ProductViewModel>($"api/products/{id}");
            response.ImagePath = $"{_imagePath}{response.ImagePath}";
            return response;
        }

        public async Task<Response> UpdateAsync(int id, UpdateProductRequest request)
        {
            var response = await _client.PutAsync($"api/products/{id}", HandleRequest(request));
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            };
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }
    }
}
