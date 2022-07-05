using Microsoft.AspNetCore.Mvc;
using VegetableShop.Api.Dto.Category;
using VegetableShop.Api.Services.Categories;

namespace VegetableShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IConfiguration _configuration;
        public CategoriesController(ICategoryService categoryService, IConfiguration configuration)
        {
            _categoryService = categoryService;
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_categoryService.GetAll());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryDto createCategoryDto)
        {
            var category = await _categoryService.CreateAsync(createCategoryDto);
            if (category.IsSuccess)
            {
                return Created(new Uri($"{_configuration["BaseAddress"]}/api/categories/{category.categoryDto.Id}"), category.categoryDto);
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsyns(int id, [FromBody] UpdateCategoryDto updateCategoryDto)
        {
            if (await _categoryService.GetCategoryByIdAsync(id) is null)
            {
                return NotFound();
            }
            var category = await _categoryService.UpdateAsync(id, updateCategoryDto);
            if (category)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (await _categoryService.GetCategoryByIdAsync(id) is null)
            {
                return NotFound();
            }
            var result = await _categoryService.DeleteAsync(id);
            if (result)
            {
                return NoContent();
            }
            return BadRequest();
        }
    }
}
