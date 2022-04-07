using EntityFramework.Data;
using EntityFramework.Models;
using EntityFramework.Services;
using EntityFramework.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly LayoutService _layoutService;
        private readonly IProductService _productService;
        public HomeController(AppDbContext context, LayoutService layoutService,IProductService productService)
        {
            _context = context;
            _layoutService = layoutService;
            _productService = productService;
        }
        public async Task<IActionResult> Index()
        {
            //HttpContext.Session.SetString("name", "Esger");
            //Response.Cookies.Append("surname", "Esgerov",new CookieOptions { MaxAge =TimeSpan.FromMinutes(5)});

            Dictionary<string, string> settings = _layoutService.GetSettings();

            int take = int.Parse(settings["HomeTake"]);

            IEnumerable<Product> products = await _productService.GetProducts(take);

            List<Slider> sliders = await _context.Sliders.ToListAsync();
            SliderDetail details = await _context.SliderDetails.FirstOrDefaultAsync();
            List<Category> categories = await _context.Categories.ToListAsync();
            //Settings settings = await _context.Settings.FirstOrDefaultAsync();
            
            About abouts = await _context.Abouts
                .Include(i=>i.Advantages)
                .FirstOrDefaultAsync();
            ExpertSection expertSection = await _context.ExpertSections.FirstOrDefaultAsync();
            List<Expert> experts = await _context.Experts
                .Include(i => i.Image)
                .ToListAsync();
            List<Expert> expertsThoughts = await _context.Experts
                .Include(i => i.Image)
                .OrderBy(m=>m.Id)
                .Take(2)
                .OrderByDescending(m=>m.Id)
                .ToListAsync();
            List<Blog> blogs = await _context.Blogs.ToListAsync();
            List<Instagram> instagrams = await _context.Instagrams.ToListAsync();
            



            HomeVM homeVM = new HomeVM
            {
                Sliders = sliders,
                Detail = details,
                Categories = categories,
                Products = products,
                Abouts = abouts,
                Experts = experts,
                ExpertsThoughts =expertsThoughts,
                ExpertSection = expertSection,
                Blogs = blogs,
                Instagrams = instagrams
            };
            return View(homeVM);
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id is null) BadRequest();

            Product dbProduct = await _context.Products.FindAsync(id);

            if (dbProduct == null) return BadRequest();

            List<BasketVM> basket;

            if (Request.Cookies["basket"] != null)
            {
                basket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                basket = new List<BasketVM>();
            }

            var existProduct = basket.Find(m => m.Id == dbProduct.Id);

            if (existProduct == null)
            {
                basket.Add(new BasketVM
                {
                    Id = dbProduct.Id,
                    Count = 1
                });
            }
            else
            {
                existProduct.Count++;
            }


            Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Test()
        {
            return Json(JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]));
        }
    }
}
