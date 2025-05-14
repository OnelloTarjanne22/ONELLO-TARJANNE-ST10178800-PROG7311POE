using PROG7311POE_ST10178800.Data;
using PROG7311POE_ST10178800.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//Based on example from (IIE,2025)

namespace PROG7311POE_ST10178800.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        // Retreives the list of farmer products using the farmer id
        public async Task<List<Product>> GetProductsByFarmerAsync(string farmerId)
        {
            

            return await _context.Products
                .Where(p => p.FarmerId == farmerId)
                .OrderByDescending(p => p.DateAdded)
                .ToListAsync();
        }
        // Additiom of product to the database and linking product to farmer using the farmer id
        public async Task AddProductAsync(Product product, string farmerId)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            if (string.IsNullOrEmpty(farmerId)) throw new ArgumentException("Farmer ID is required");

            if (product.DateAdded == default(DateTime))
            {
                product.DateAdded = DateTime.Now;
            }
            product.FarmerId = farmerId;

            _context.Products.Add(product);
            await _context.SaveChangesAsync(); 
        }
        //Deletion of the products using the product id and the farmer id
        public async Task DeleteProductAsync(int productId, string farmerId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId && p.FarmerId == farmerId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        // Filters the products and provides a list based on the chosen filter
        public async Task<List<Product>> FilterProductsAsync(string category, DateTime? from, DateTime? to)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            if (from.HasValue)
                query = query.Where(p => p.DateAdded >= from);

            if (to.HasValue)
                query = query.Where(p => p.DateAdded <= to);

            return await query.ToListAsync();
        }
    }
}