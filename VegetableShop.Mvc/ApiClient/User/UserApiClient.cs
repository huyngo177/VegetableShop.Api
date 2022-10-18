using AutoMapper;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VegetableShop.Api.Dto.Page;
using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Page;
using VegetableShop.Mvc.Models.User;

namespace VegetableShop.Mvc.ApiClient.User
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public UserApiClient(IConfiguration configuration,
            IHttpClientFactory httpClientFactory, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, mapper, httpContextAccessor)
        {
            _configuration = configuration;
            _clientFactory = httpClientFactory;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _client = _clientFactory.CreateClient();
            _client.BaseAddress = new Uri($"{_configuration["BaseAddress"]}");
        }

        public async Task<PaginationResult<UserViewModel>> GetAllAsync(PageViewModel request)
        {
            return await GetAsync<PaginationResult<UserViewModel>>(
                $"api/users/page?" +
                $"PageIndex={request.PageIndex}" +
                $"&PageSize={request.PageSize}" +              
                $"&SortProperty={request.SortProperty}" +
                $"&SortOrder={request.SortOrder}"+
                $"&Keyword={request.Keyword}" +
                $"&IsLocked={request.IsLocked}" );
        }

        public async Task<UserViewModel> GetUserByIdAsync(int id)
        {
            return await GetAsync<UserViewModel>($"api/users/{id}");
        }

        public async Task<UserViewModel> GetUserByNameAsync(string username)
        {

            return await GetAsync<UserViewModel>($"api/users/username?username={username}");
        }

        public async Task<Response> Login(LoginRequest request)
        {
            var response = await _client.PostAsync("api/users/login", HandleRequest(request));
            return JsonConvert.DeserializeObject<Response>(await response.Content.ReadAsStringAsync());
        }
        public async Task<CreateResponse> CreateAsync(CreateUserRequest request)
        {
            var response = await _client.PostAsync("api/users", HandleRequest(request));
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CreateResponse>(data);
            }
            var body = JsonConvert.DeserializeObject<CreateResponse>(response.Content.ReadAsStringAsync().Result);
            body.IsSuccess = false;
            return body;
        }

        public async Task<Response> UpdateAsync(int id, UpdateUserRequest request)
        {
            var sessions = _session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await _client.PutAsync($"api/users/{id}", HandleRequest(request));
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            };
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<Response> UpdatePasswordAsync(int id, UpdatePasswordRequest request)
        {
            var sessions = _session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await _client.PutAsync($"api/users/{id}/update-password", HandleRequest(request));
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            };
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<Response> LockAsync(int id)
        {
            var sessions = _session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await _client.GetAsync($"api/users/{id}/lock");
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var sessions = _session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await _client.DeleteAsync($"api/users/{id}");
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task RevokeAsync()
        {
            var sessions = _session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            await _client.GetAsync($"api/users/revokes");
        }

        public async Task<Response> RestoreLockedStatusAsync(int id)
        {
            var sessions = _session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await _client.GetAsync($"api/users/{id}/restore-locked-status");
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<Response> AssignRoleAsync(int id, string role)
        {
            var sessions = _session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await _client.PutAsync($"api/users/{id}/assign-role", HandleRequest(role));
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<Response> RemoveRoleAsync(int id, string role)
        {
            var sessions = _session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await _client.PostAsync($"api/users/{id}/remove-role", HandleRequest(role));
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<Response> RevokeByUsernameAsync(string username)
        {
            var sessions = _session.GetString("Token");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessions);
            var response = await _client.PostAsync($"api/users/revoke/{username}", HandleRequest(username));
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<IEnumerable<string>> GetRolesByUserIdAsync(int id)
        {
            return await GetAsync<IList<string>>($"api/users/{id}/roles");
        }
    }
}
