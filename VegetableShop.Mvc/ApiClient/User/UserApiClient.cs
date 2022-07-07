﻿using AutoMapper;
using Newtonsoft.Json;
using VegetableShop.Api.Dto.User;
using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.User;

namespace VegetableShop.Mvc.ApiClient.User
{
    public class UserApiClient : BaseApiClient, IUserApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public UserApiClient(IConfiguration configuration, IHttpClientFactory httpClientFactory, IMapper mapper)
            : base(configuration, httpClientFactory, mapper)
        {
            _configuration = configuration;
            _clientFactory = httpClientFactory;
            _mapper = mapper;
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri($"{configuration["BaseAddress"]}");
        }

        public async Task<IEnumerable<UserViewModel>> GetAllAsync()
        {
            return await GetAsync<IEnumerable<UserViewModel>>("api/users");
        }

        public async Task<UserViewModel> GetUserByIdAsync(int id)
        {
            return await GetAsync<UserViewModel>($"api/users/{id}");
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
            var body = JsonConvert.DeserializeObject<CreateResponse>(await response.Content.ReadAsStringAsync());
            body.IsSuccess = false;
            return body;

        }

        public async Task<Response> UpdateAsync(int id, UpdateUserRequest request)
        {
            var response = await _client.PutAsync($"api/users/{id}", HandleRequest(request));
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            };
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }

        public async Task<Response> DeleteAsync(int id)
        {
            var response = await _client.DeleteAsync($"api/users/{id}");
            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            var body = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<Response>(body);
        }
    }
}
