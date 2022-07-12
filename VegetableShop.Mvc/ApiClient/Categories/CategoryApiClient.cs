using AutoMapper;
using Newtonsoft.Json;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Categories;

namespace VegetableShop.Mvc.ApiClient.Categories
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
        }
        public async Task<IEnumerable<CategoryViewModel>> GetAllAsync()
        {
            return await GetAsync<IEnumerable<CategoryViewModel>>("api/categories");
        }

        public async Task<IList<Category>> SelectAll()
        {
            return await GetAsync<IList<Category>>("api/categories");
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"api/categories/{id}");
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<CreateResponse> CreateAsync(CreateCategoryRequest request)
        {
            var response = await _client.PostAsync("api/categories", HandleRequest(request));
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CreateResponse>(data);
            }
            var body = JsonConvert.DeserializeObject<CreateResponse>(await response.Content.ReadAsStringAsync());
            body.IsSuccess = false;
            return body;
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int id)
        {
            var response = await GetAsync<CategoryViewModel>($"api/categories/{id}");
            return response;
        }

        public async Task<Response> UpdateAsync(int id, UpdateCategoryRequest request)
        {
            var response = await _client.PutAsync($"api/categories/{id}", HandleRequest(request));
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            };
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }
    }
}
