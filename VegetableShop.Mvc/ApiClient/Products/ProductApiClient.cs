﻿using AutoMapper;
using Newtonsoft.Json;
using VegetableShop.Api.Dto.Page;
using VegetableShop.Mvc.Models;
using VegetableShop.Mvc.Models.Products;

namespace VegetableShop.Mvc.ApiClient.Products
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductApiClient(IConfiguration configuration,
            IHttpClientFactory httpClientFactory, IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, mapper, httpContextAccessor)
        {
            _configuration = configuration;
            _clientFactory = httpClientFactory;
            _mapper = mapper;
            _client = _clientFactory.CreateClient();
            _client.BaseAddress = new Uri($"{_configuration["BaseAddress"]}");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateResponse> CreateAsync(CreateProductRequest request)
        {
            var requestContent = new MultipartFormDataContent();
            if (request.Image is not null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.Image.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.Image.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "Image", request.Image.FileName);
            }
            requestContent.Add(new StringContent(request.Price.ToString()), "price");
            requestContent.Add(new StringContent(request.Stock.ToString()), "stock");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Name) ? "" : request.Name.ToString()), "name");
            requestContent.Add(new StringContent(string.IsNullOrEmpty(request.Description) ? "" : request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.CategoryId.ToString()), "categoryId");

            var response = await _client.PostAsync("api/products", requestContent);
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

        public async Task<PageResult<ProductViewModel>> GetAllAsync(GetProductPageRequest request)
        {
            var products = await GetContainDateAsync<PageResult<ProductViewModel>>(
                $"api/products/page?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&categoryId={request.CategoryId}&status={request.Status}");
            return products;
        }

        public async Task<ProductViewModel> GetProductByIdAsync(int id)
        {
            var product = await GetContainDateAsync<ProductViewModel>($"api/products/{id}");
            return product;
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

        public async Task<IEnumerable<ProductViewModel>> GetProductByCategoryIdAsync(int id)
        {
            return await GetContainDateAsync<IEnumerable<ProductViewModel>>($"api/products/categories/{id}");
        }
    }
}
