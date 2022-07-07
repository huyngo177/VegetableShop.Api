using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Mvc.ApiClient.Products;
using VegetableShop.Mvc.Models.Products;

namespace VegetableShop.Mvc.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IMapper _mapper;
        public ProductController(IProductApiClient productApiClient, IMapper mapper)
        {
            _productApiClient = productApiClient;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productApiClient.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            return View(await _productApiClient.GetProductByIdAsync(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var response = await _productApiClient.CreateAsync(request);
            if (response.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productApiClient.GetProductByIdAsync(id);
            return View(_mapper.Map<UpdateProductRequest>(product));
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateProductRequest request)
        {
            var response = await _productApiClient.UpdateAsync(id, request);
            if (response.IsSuccess)
            {
                return View("Detail", new { id = id });
            }
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productApiClient.DeleteAsync(id);
            if (response.IsSuccess)
            {
                return View("Index");
            }
            return View("Index", new { message = response.Message });
        }
    }
}
