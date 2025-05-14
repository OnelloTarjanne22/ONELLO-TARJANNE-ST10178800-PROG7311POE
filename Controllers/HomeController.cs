using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PROG7311POE_ST10178800.Models;
using System.Diagnostics;

namespace PROG7311POE_ST10178800.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {//Log for when a user is directed to  Index page
            _logger.LogInformation("Navigated to Index page.");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult ViewProducts()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Employee")]
        public IActionResult AddFarmer()
        {
            return View();
        }

        [Authorize(Roles = "Farmer")]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FilterProducts()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ProductList()
        {
            return View();
        }
        [HttpGet]
        public IActionResult FarmerDetails()
        {
            return View();
        }
        [HttpPost]
        public IActionResult FilterProducts(string category, DateTime? fromDate, DateTime? toDate)
        {
            // Logic to filter products based on the parameters
            return RedirectToAction("FilteredResults"); 
        }

        public IActionResult FilteredResults()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

