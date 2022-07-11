using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Mvc.ApiClient.Categories;
using VegetableShop.Mvc.Models.Role;

namespace VegetableShop.Mvc.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleApiClient _roleApiClient;
        private readonly IMapper _mapper;
        public RoleController(IRoleApiClient categoryApiClient, IMapper mapper)
        {
            _roleApiClient = categoryApiClient;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _roleApiClient.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleRequest request)
        {
            var response = await _roleApiClient.CreateAsync(request);
            if (response.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _roleApiClient.GetCategoryByIdAsync(id);
            return View(_mapper.Map<UpdateRoleRequest>(product));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, UpdateRoleRequest request)
        {
            var response = await _roleApiClient.UpdateAsync(id, request);
            if (response.IsSuccess)
            {
                return View("Detail", new { id = id });
            }
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _roleApiClient.DeleteAsync(id);
            if (response.IsSuccess)
            {
                TempData["Message"] = "Delete role success";
                return RedirectToAction("Index");
            }
            TempData["Message"] = "Delete role fail";
            return RedirectToAction("Index", new { message = response.Message });
        }
    }
}
