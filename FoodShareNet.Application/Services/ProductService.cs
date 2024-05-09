using FoodShareNet.Application.Interfaces;
using FoodShareNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IFoodShareDbContext _context;
        public ProductService(IFoodShareDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<IList<Product>> GetAllProductsAsync()
        {
            var products = await _context.Products
            .ToListAsync();
            return products;
        }
    }
}
