﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Web.Services;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductApiService _productApiService;
        private readonly CategoryApiService _categoryApiService;
        public ProductsController(ProductApiService productApiService, CategoryApiService categoryApiService)
        {
            _productApiService = productApiService;
            _categoryApiService = categoryApiService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _productApiService.GetProductsWithCategoryAsync());
        }

        public async Task<IActionResult> Save()
        {
            var categories = await _categoryApiService.GetAllAsync();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                await _productApiService.SaveAsync(productDTO);

                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryApiService.GetAllAsync();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View();
        }

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productApiService.GetByIdAsync(id);
            var categories = await _categoryApiService.GetAllAsync();

            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }

        [HttpPost]

        public async Task<IActionResult> Update(ProductDTO productDTO)
        {
            if (ModelState.IsValid)
            {
                await _productApiService.UpdateAsync(productDTO);
                return RedirectToAction(nameof(Index));
            }
            var categories = await _categoryApiService.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", productDTO.CategoryId);

            return View(productDTO);
        }

        public async Task<IActionResult> Remove(int id)
        {
            await _productApiService.RemoveAsync(id);

            return RedirectToAction(nameof(Index));

        }
    }
}
