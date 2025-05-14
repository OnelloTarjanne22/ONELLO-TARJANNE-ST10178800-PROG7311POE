using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROG7311POE_ST10178800.Models;
using PROG7311POE_ST10178800.Services;
////Adapted from tutorial by (tutorialsEU,2023)
namespace PROG7311POE_ST10178800.Controllers
{
    [Authorize(Roles = "Employee")]// Only for persons in the employee role
    public class EmployeeController : Controller
    {
        private readonly IFarmerService _farmerService;
        private readonly IProductService _productService;
        private readonly UserManager<Employee> _userManager;
        private readonly IUserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;


        public EmployeeController(
            IFarmerService farmerService, 
            IProductService productService,
            UserManager<Employee> userManager,
            IUserService userService, RoleManager<IdentityRole> roleManager)
        {
            _farmerService = farmerService;
            _productService = productService;
            _userManager = userManager;
            _userService = userService;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            // Check if user is authenticated and has Employee role
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Employee"))
            {
                return RedirectToAction("AccessDenied", "Account");
            }

            var farmers = await _farmerService.GetAllFarmersAsync();
            return View(farmers);
        }
        //Method to view the products of a specific farmer as an employee
        [HttpGet]
        public async Task<IActionResult> ViewProducts(string farmerId, string category, DateTime? fromDate, DateTime? toDate)
        {
            if (string.IsNullOrEmpty(farmerId))
            {
                return NotFound("Farmer ID is missing.");
            }

            var products = await _productService.GetProductsByFarmerAsync(farmerId);

            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category != null && p.Category.Contains(category, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (fromDate.HasValue)
            {
                products = products.Where(p => p.DateAdded >= fromDate.Value).ToList();
            }

            if (toDate.HasValue)
            {
                products = products.Where(p => p.DateAdded <= toDate.Value).ToList();
            }

            ViewBag.FarmerId = farmerId;
            return View("ViewProducts", products);
        }




        [HttpGet]
        public IActionResult AddFarmer()
        {
            return View();
        }
        //Method to add farmer profile while logged in as employee
        [HttpPost]
        public async Task<IActionResult> AddFarmer(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //Creating the new employee profile with essesntial details
            var user = new Employee
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                Role = "Farmer"
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Farmer"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Farmer"));
                }

                await _userManager.AddToRoleAsync(user, "Farmer");

                // Add to Farmers table
                var farmer = new Farmer
                {
                    FullName = model.FullName,
                    Email = model.Email,
                     UserId = user.Id
                };
                await _farmerService.AddFarmerAsync(farmer);

                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }



        [HttpGet]
        public IActionResult FilterProducts()
        {
            return View();
        }
        // Method to direct users to the filter products page after their selection of filters
        [HttpPost]
        public async Task<IActionResult> FilterProducts(string category, DateTime? fromDate, DateTime? toDate)
        {
            var products = await _productService.FilterProductsAsync(category, fromDate, toDate);
            return View("FilteredResults", products);
        }
    }
}