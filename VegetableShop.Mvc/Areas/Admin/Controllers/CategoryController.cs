using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Mvc.ApiClient.Categories;
using VegetableShop.Mvc.Models.Categories;

namespace VegetableShop.Mvc.Areas.Admin.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IMapper _mapper;
        public CategoryController(ICategoryApiClient categoryApiClient, IMapper mapper)
        {
            _categoryApiClient = categoryApiClient;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _categoryApiClient.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryRequest request)
        {
            var response = await _categoryApiClient.CreateAsync(request);
            if (response.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _categoryApiClient.GetCategoryByIdAsync(id);
            return View(_mapper.Map<UpdateCategoryRequest>(product));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateCategoryRequest request)
        {
            var response = await _categoryApiClient.UpdateAsync(id, request);
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
            var response = await _categoryApiClient.DeleteAsync(id);
            if (response.IsSuccess)
            {
                TempData["Message"] = "Delete category success";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Delete category fail";
            return RedirectToAction("Index", new { message = response.Message });
        }
    }
}
