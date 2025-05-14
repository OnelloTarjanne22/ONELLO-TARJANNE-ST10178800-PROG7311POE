using Microsoft.AspNetCore.Mvc;
using PROG7311POE_ST10178800.Models;
using PROG7311POE_ST10178800.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PROG7311POE_ST10178800.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> FilterProducts(DateTime? fromDate, DateTime? toDate, string searchTerm)
        {
            var productsQuery = _context.Products.AsQueryable();

            if (fromDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.DateAdded >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.DateAdded <= toDate.Value);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(searchTerm));
            }

            var filteredProducts = await productsQuery.ToListAsync();
            return View("FilterProducts", filteredProducts);
        }

        [HttpGet]
        public async Task<IActionResult> FilterProducts()
        {
            var categories = await _context.Products
                .Select(p => p.Category)
                .Distinct()
                .ToListAsync();

            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(product);
        }
    }
}
