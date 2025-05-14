using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROG7311POE_ST10178800.Models;
using PROG7311POE_ST10178800.Services;

namespace PROG7311POE_ST10178800.Controllers
{
    [Authorize(Roles = "Farmer")]
    public class FarmerController : Controller
    {
        private readonly IProductService _productService;
        private readonly UserManager<Employee> _userManager;

        public FarmerController(
            IProductService productService,
            UserManager<Employee> userManager)
        {
            _productService = productService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Check if user is authenticated and has Farmer role
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Farmer"))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            // Get the current user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get products for this farmer
            var products = await _productService.GetProductsByFarmerAsync(user.Id);
            return View(products);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            await _productService.DeleteProductAsync(id, user.Id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                await _productService.AddProductAsync(product, user.Id);
                return RedirectToAction("Index");
            }
            return View(product);
        }
    }
}