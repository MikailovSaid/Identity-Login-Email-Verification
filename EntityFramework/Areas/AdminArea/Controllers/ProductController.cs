using EntityFramework.Data;
using EntityFramework.Models;
using EntityFramework.Utilities.Helpers;
using EntityFramework.Utilities.Pagination;
using EntityFramework.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.Utilities.Files;

namespace EntityFramework.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _enviroment;
        public ProductController(AppDbContext context, IWebHostEnvironment enviroment)
        {
            _context = context;
            _enviroment = enviroment;
        }
        public async Task<IActionResult> Index(int page = 1, int take = 10)
        {
            var products = await _context.Products
                .Include(m => m.Images)
                .Include(m => m.Category)
                .Where(m=>!m.IsDeleted)
                .OrderByDescending(m => m.Id)
                .Skip((page - 1) * take)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();

            var productsVM = GetMapDatas(products);

            int count = await GetPageCount(take);

            Paginate<ProductListVM> result = new Paginate<ProductListVM>(productsVM, page, count);

            return View(result);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await GetCategoriesByProduct();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM productVM)
        {
            ViewBag.categories = await GetCategoriesByProduct();
            if (!ModelState.IsValid) return View();

            List<ProductImage> imageList = new List<ProductImage>();

            if (productVM.Photos != null)
            {
                foreach (var photo in productVM.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = Helper.GetFilePath(_enviroment.WebRootPath, "img", fileName);

                    //Using Statement note this
                    await photo.SaveFiles(path);

                    ProductImage dbProductImage = new ProductImage
                    {
                        Image = fileName
                    };

                    imageList.Add(dbProductImage);
                }

                imageList.FirstOrDefault().IsMain = true;
            }



            Product product = new Product
            {
                Name = productVM.Name,
                Count = productVM.Count,
                Price = productVM.Price,
                CategoryId = productVM.CategoryId,
                Images = imageList
            };

            await _context.ProductImages.AddRangeAsync(imageList);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();



            return RedirectToAction(nameof(Index));
        }


        #region Delete with view
        //public async Task<IActionResult> Delete(int id)
        //{
        //    return Ok();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[ActionName("Delete")]
        //public async Task<IActionResult> DeleteItem(int id)
        //{

        //}
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Product products = await _context.Products.Include(m=>m.Images).Where(m=>m.Id==id  && !m.IsDeleted).FirstOrDefaultAsync();
            if (products is null) return NotFound();

            foreach (var product in products.Images)
            {
                string path = Helper.GetFilePath(_enviroment.WebRootPath, "img", product.Image);

                Helper.DeleteFile(path);

                product.IsDeleted = true;

            }

            products.IsDeleted = true;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.categories = await GetCategoriesByProduct();
            Product products = await _context.Products.Include(m => m.Images).Include(m=>m.Category).Where(m => m.Id == id && !m.IsDeleted).FirstOrDefaultAsync();
            if (products is null) return NotFound();



            ProductUpdateVM productResult = new ProductUpdateVM
            {
                Id = products.Id,
                Name = products.Name,
                Count = products.Count,
                CategoryId = products.CategoryId,
                Images = products.Images,
                Price = products.Price
            };

            

            return View(productResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductUpdateVM productUpdateVM)
        {
            ViewBag.categories = await GetCategoriesByProduct();
            if (!ModelState.IsValid) return View(productUpdateVM);
            Product products = await _context.Products
                .Include(m => m.Images)
                .Include(m => m.Category)
                .Where(m => m.Id == id && !m.IsDeleted)
                .FirstOrDefaultAsync();
            if (products is null) return NotFound();

            List<ProductImage> imageList = new List<ProductImage>();

            if(productUpdateVM.Photos != null)
            {
                foreach (var images in products.Images)
                {
                    string path = Helper.GetFilePath(_enviroment.WebRootPath, "img", images.Image);

                    Helper.DeleteFile(path);

                    images.IsDeleted = true;

                }


                foreach (var photo in productUpdateVM.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = Helper.GetFilePath(_enviroment.WebRootPath, "img", fileName);

                    await photo.SaveFiles(path);

                    ProductImage productImage = new ProductImage
                    {
                        Image = fileName
                    };

                    imageList.Add(productImage);
                }

                imageList.FirstOrDefault().IsMain = true;
                products.Images = imageList;

            }

            products.Name = productUpdateVM.Name;
            products.CategoryId = productUpdateVM.CategoryId;
            products.Count = productUpdateVM.Count;
            products.Price = productUpdateVM.Price;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SetDefaultImage(DefaultImageVM model)
        {
            var produtcImages = await _context.ProductImages.Where(m => m.ProductId == model.ProductId).ToListAsync();
            foreach (var image in produtcImages)
            {
                if (image.Id == model.ImageId)
                {
                    image.IsMain = true;
                }
                else
                {
                    image.IsMain = false;
                } 
            }
            await _context.SaveChangesAsync();
            return Ok(produtcImages);
        }

        private async Task<SelectList> GetCategoriesByProduct()
        {
            var categories = await _context.Categories.Where(m => !m.IsDeleted).ToListAsync();
            return new SelectList(categories, "Id", "Name");
        }


        //For Paginate
        private async Task<int> GetPageCount(int take)
        {
            var count = await _context.Products.CountAsync();

            return (int)Math.Ceiling((decimal)count / take);
        }


        //For equality between VM and Model
        private List<ProductListVM> GetMapDatas(List<Product> products)
        {
            List<ProductListVM> productList = new List<ProductListVM>();
            foreach (var product in products)
            {
                ProductListVM newProduct = new ProductListVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Image = product.Images.Where(m => m.IsMain).FirstOrDefault()?.Image,
                    CategoryName = product.Category.Name,
                    Count = product.Count,
                    Price = product.Price
                };

                productList.Add(newProduct);
            }

            return productList;
        }
    }

}
