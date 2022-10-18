using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Api.Dto.Page;
using VegetableShop.Mvc.ApiClient.Products;
using VegetableShop.Mvc.Models.Products;

namespace VegetableShop.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IMapper _mapper;
        public ProductController(IProductApiClient productApiClient, IMapper mapper)
        {
            _productApiClient = productApiClient;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string keyword, int? categoryId, int pageIndex = 1, int pageSize = 4)
        {
            var request = new GetProductPageRequest()
            {
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                CategoryId = categoryId
            };
            ViewBag.Keyword = keyword;
            return View(await _productApiClient.GetAllAsync(request));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            return View(await _productApiClient.GetProductByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var response = await _productApiClient.CreateAsync(request);
            if (response.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productApiClient.GetProductByIdAsync(id);
            return View(_mapper.Map<UpdateProductRequest>(product));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateProductRequest request)
        {
            var response = await _productApiClient.UpdateAsync(id, request);
            if (response.IsSuccess)
            {
                return View("Detail", new { id });
            }
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _productApiClient.DeleteAsync(id);
            if (response.IsSuccess)
            {
                TempData["Message"] = "Delete product success";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Delete product fail";
            return RedirectToAction("Index", new { message = response.Message });
        }
    }
}
