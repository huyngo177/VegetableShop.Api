using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Net.Http.Headers;
using System.Text;

namespace VegetableShop.Mvc.ApiClient
{
    public class BaseApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public BaseApiClient(IConfiguration configuration, 
            IHttpClientFactory httpClientFactory, 
            IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _client = _httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri($"{_configuration["BaseAddress"]}");
        }
        public static HttpContent HandleRequest<T>(T request)
        {
            var json = JsonConvert.SerializeObject(request);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        public async Task<T> GetContainDateAsync<T>(string url)
        {
            var sessions = _session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await _client.GetAsync(url);
            var format = "HH:mm:ss dd/MM/yyyy";
            var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync(), dateTimeConverter);
        }
        public async Task<T> GetAsync<T>(string url)
        {
            var sessions = _session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await _client.GetAsync(url);
            return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
        }
    }
}
