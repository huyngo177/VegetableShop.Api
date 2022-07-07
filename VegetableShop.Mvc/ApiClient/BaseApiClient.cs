using AutoMapper;
using Newtonsoft.Json;
using System.Text;

namespace VegetableShop.Mvc.ApiClient
{
    public class BaseApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        public BaseApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _client = _httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri($"{_configuration["BaseAddress"]}");
        }
        public static HttpContent HandleRequest<T>(T request)
        {
            var json = JsonConvert.SerializeObject(request);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _client.GetAsync(url);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}
