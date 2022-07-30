using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Api.Common;
using VegetableShop.Api.Dto.Page;
using VegetableShop.Api.Dto.Products;
using VegetableShop.Api.Services.Products;

namespace VegetableShop.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IConfiguration _configuration;
        private readonly string _imagePath;

        public ProductsController(IProductService productService, IConfiguration configuration)
        {
            _productService = productService;
            _configuration = configuration;
            _configuration = configuration;
            _imagePath = $"{_configuration["BaseAddress"]}";
        }

        [HttpGet("page")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] GetProductPageRequest request)
        {
            return Ok(await _productService.GetAllAsync(request));
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                return BadRequest();
            }
            var product = await _productService.GetProductByIdAsync(id);
            if (product is null)
            {
                return NotFound();
            }
            product.ImagePath = $"{_imagePath}{product.ImagePath}";
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateProductDto createProductDto)
        {
            var product = await _productService.CreateAsync(createProductDto);
            if (product.IsSuccess)
            {
                return Created(new Uri($"{_configuration["BaseAddress"]}/api/products/{product.productDto.Id}"), product.productDto);
            }
            return BadRequest(product.Message);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] UpdateProductDto updateProductDto)
        {
            if (await _productService.GetProductByIdAsync(id) is null)
            {
                return NotFound();
            }
            var product = await _productService.UpdateAsync(id, updateProductDto);
            if (product)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (await _productService.GetProductByIdAsync(id) is null)
            {
                return NotFound();
            }
            if (await _productService.ChangeStatusProductAsync(id, Status.Unavailable))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPost("restore/{id}")]
        public async Task<IActionResult> RestoreAsync(int id)
        {
            if (await _productService.GetProductByIdAsync(id) is null)
            {
                return NotFound();
            }
            if (await _productService.ChangeStatusProductAsync(id, Status.Available))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
