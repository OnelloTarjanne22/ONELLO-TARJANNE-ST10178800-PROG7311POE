using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROG7311POE_ST10178800.Models;
using PROG7311POE_ST10178800.Services;

namespace PROG7311POE_ST10178800.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Employee> _userManager;
        private readonly SignInManager<Employee> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserService _userService;
        private readonly IFarmerService _farmerService;

        public AccountController(
            UserManager<Employee> userManager,
            SignInManager<Employee> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserService userService,
            IFarmerService farmerService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userService = userService;
            _farmerService = farmerService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string fullName, string role)
        {
            // Validate role
            if (role != "Employee" && role != "Farmer")
            {
                ModelState.AddModelError("", "Invalid role selected");
                return View();
            }

            var user = new Employee
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                Role = role
            };

            // Create the user
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // Ensure role exists and add user to role
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }

                await _userManager.AddToRoleAsync(user, role);
                // If user is a Farmer, add to Farmers table
                if (role == "Farmer")
                {
                    var farmer = new Farmer
                    {
                        FullName = fullName,
                        Email = email,
                        UserId = user.Id
                    };
                    await _farmerService.AddFarmerAsync(farmer);
                }

                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);

                // Redirect based on role
                return RedirectToAction("Index", role == "Farmer" ? "Farmer" : "Employee");
            }

            // If we got this far, something failed, show errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe = false)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required");
                return View();
            }

            // This doesn't check roles yet, just if the password is valid
            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Find user to determine role for redirect
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    // This shouldn't happen if login succeeded
                    return RedirectToAction("Index", "Home");
                }

                // Check which role the user belongs to
                bool isFarmer = await _userManager.IsInRoleAsync(user, "Farmer");
                bool isEmployee = await _userManager.IsInRoleAsync(user, "Employee");

                if (isFarmer)
                {
                    return RedirectToAction("Index", "Farmer");
                }
                else if (isEmployee)
                {
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    // User has no specific role, redirect to home
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt. Please check your email and password.");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}