using EntityFramework.Data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetProducts(int take)
        {
            try
            {
                List<Product> products = await  _context.Products
                    .Include(m => m.Category)
                    .Include(m => m.Images)
                    .OrderByDescending(m => m.Id)
                    .Skip(1)
                    .Take(take)
                    .ToListAsync();
                 return products;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
