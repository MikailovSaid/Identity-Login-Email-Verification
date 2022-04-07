using EntityFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Services
{
    public interface IProductService
    {
       Task<IEnumerable<Product>> GetProducts(int take);
    }
}
