using AutoMapper;
using Newtonsoft.Json;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Role;

namespace VegetableShop.Mvc.ApiClient.Role
{
    public class RoleApiClient : BaseApiClient, IRoleApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        public RoleApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IMapper mapper)
            : base(configuration, httpClientFactory, mapper)
        {
        }
        public async Task<IEnumerable<RoleViewModel>> GetAllAsync()
        {
            return await GetAsync<IEnumerable<RoleViewModel>>("api/roles");
        }

        public async Task<IList<AppRole>> SelectAll()
        {
            return await GetAsync<IList<AppRole>>("api/roles");
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"api/roles/{id}");
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<CreateResponse> CreateAsync(CreateRoleRequest request)
        {
            var response = await _client.PostAsync("api/roles", HandleRequest(request));
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CreateResponse>(data);
            }
            var body = JsonConvert.DeserializeObject<CreateResponse>(await response.Content.ReadAsStringAsync());
            body.IsSuccess = false;
            return body;
        }

        public async Task<RoleViewModel> GetCategoryByIdAsync(int id)
        {
            var response = await GetAsync<RoleViewModel>($"api/roles/{id}");
            return response;
        }

        public async Task<Response> UpdateAsync(int id, UpdateRoleRequest request)
        {
            var response = await _client.PutAsync($"api/roles/{id}", HandleRequest(request));
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            };
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }
    }
}
