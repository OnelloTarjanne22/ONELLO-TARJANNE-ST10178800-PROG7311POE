using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PROG7311POE_ST10178800.Models;
using PROG7311POE_ST10178800.Services;
//Adapted from tutorial by (Tech & Code with Phi,2022)
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
        //Method for registration of a user
        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string fullName, string role)
        {
            // Validation of role
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

                // Redirect based on role to respective page after sign in
                return RedirectToAction("Index", role == "Farmer" ? "Farmer" : "Employee");
            }

           // Displays when an error has occurred in the registration
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
        // Login method for users who have registered
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe = false)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Email and password are required");
                return View();
            }

            // Password validation for a user already signed in
            var result = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Find user to determine role for redirect
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    //when login fails
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
                    //If the user has no role, redirect to home
                    return RedirectToAction("Index", "Home");
                }
            }
            //Incorrect email or password prompt
            ModelState.AddModelError("", "Invalid login attempt. Please check your email and password.");
            return View();
        }
        //Method to log out of the account
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");//Directed to home page after logging out
        }
        //Should a user try to access an a page outside their role
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}