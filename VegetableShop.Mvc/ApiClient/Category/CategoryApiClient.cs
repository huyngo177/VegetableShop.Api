using AutoMapper;
using VegetableShop.Mvc.Models.Category;

namespace VegetableShop.Mvc.ApiClient.Category
{
    public class CategoryApiClient : BaseApiClient, ICategoryApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        public CategoryApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IMapper mapper)
            : base(configuration, httpClientFactory, mapper)
        {
            _configuration = configuration;
            _clientFactory = httpClientFactory;
            _mapper = mapper;
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri($"{_configuration["BaseAddress"]}");
        }
        public async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
        {
            return await GetAsync<IEnumerable<CategoryViewModel>>("api/categories");
        }
    }
}
