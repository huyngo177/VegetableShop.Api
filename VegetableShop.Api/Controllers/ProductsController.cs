﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VegetableShop.Api.Dto.Products;
using VegetableShop.Api.Services.Products;

namespace VegetableShop.Api.Controllers
{
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
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetAll());
        }

        [HttpGet("{id}")]
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
            if (await _productService.DeLeteAsync(id))
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpGet("categories/{categoryId}")]
        public async Task<IActionResult> GetProductByCategoryIdAsync(int categoryId)
        {
            var products = await _productService.GetProductByCategoryIdAsync(categoryId);
            if (products is not null)
            {
                return Ok(products);
            }
            return BadRequest();
        }
    }
}
