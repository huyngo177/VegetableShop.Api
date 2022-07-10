﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VegetableShop.Api.Data.Entities;
using VegetableShop.Mvc.ApiClient.Categories;
using VegetableShop.Mvc.ApiClient.Products;
using VegetableShop.Mvc.Models.Products;

namespace VegetableShop.Mvc.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoryApiClient _categoryApiClient;
        private readonly IMapper _mapper;
        public ProductController(IProductApiClient productApiClient, IMapper mapper, ICategoryApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _mapper = mapper;
            _categoryApiClient = categoryApiClient;
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
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryApiClient.GetAllAsync();
            ViewBag.Categories = categories;
            return View(categories);
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
                return View("Detail", new { id = id });
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
                return View();
            }
            TempData["Message"] = "Delete product fail";
            return RedirectToAction("Index", new { message = response.Message });
        }
    }
}
