using EntityFramework.Data;
using EntityFramework.Models;
using EntityFramework.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Controllers
{
    public class ProductController1 : Controller
    {
        private readonly AppDbContext _context;
        public ProductController1(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            Settings settings = await _context.Settings.FirstOrDefaultAsync();
            ViewBag.ProductCount = _context.Products.Where(p=>p.IsDeleted == false).Count();
            List<Product> products = await _context.Products
               .Include(m => m.Category)
               .Include(m => m.Images)
               .OrderByDescending(m => m.Id)
               .Skip(1)
               .Take(8)
               .ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> LoadMore(int skip)
        {
            Settings settings = await _context.Settings.FirstOrDefaultAsync();
            List<Product> products = await _context.Products
               .Include(m => m.Category)
               .Include(m => m.Images)
               .OrderByDescending(m => m.Id)
               .Skip(skip)
               .Take(4)
               .ToListAsync();

            return PartialView("_ProductsPartialView", products);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null) BadRequest();

            Product dbProduct = await GetProductById(id);

            if (dbProduct == null) return BadRequest();

            List<BasketVM> basket = GetBasket();

            UpdateBasket(basket, dbProduct);

            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));

            return RedirectToAction("Index", "ProductController1");
        }


        //For Add Basket
        private List<BasketVM> GetBasket()
        {
            List<BasketVM> basket;
            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }

            return basket;
        }

        private void UpdateBasket(List<BasketVM> basket, Product product)
        {
            var existProduct = basket.Find(m => m.Id == product.Id);

            if (existProduct == null)
            {
                basket.Add(new BasketVM
                {
                    Id = product.Id,
                    Count = 1
                });
            }
            else
            {
                existProduct.Count++;
            }
        }

        private async Task<Product> GetProductById(int? id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<IActionResult> Basket()
        {
            List<BasketVM> basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            List<BasketDetailVM> basketDetailsItem = new List<BasketDetailVM>();

            foreach (BasketVM item in basket)
            {
                Product product = await _context.Products.Include(m => m.Images).FirstOrDefaultAsync(m => m.Id == item.Id);

                BasketDetailVM basketDetail = new BasketDetailVM
                {
                    Id = item.Id,
                    ProductName = product.Name,
                    ProductImage = product.Images.Where(m=> m.IsMain).FirstOrDefault().Image,
                    Count = item.Count,
                    Price = product.Price * item.Count
                };

                basketDetailsItem.Add(basketDetail);
            }
            return View(basketDetailsItem);
            //return Json(JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]));
        }
    }
}
