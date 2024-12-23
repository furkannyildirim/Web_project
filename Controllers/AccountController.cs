using BooksStore.Data;
using BooksStore.Models;
using BooksStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.Controllers
{
    public class AccountController: Controller
    {
        private readonly string CUSTOMER_ROLE = "Customer"; // Define the role for customers
        private readonly SignInManager<IdentityUser> _signInManager;  
        private readonly UserManager<IdentityUser> _userManager;

        private readonly DataContext _context;

        // Initializes the SignInManager, UserManager, and DataContext for handling user authentication and database interaction
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, DataContext storeDbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = storeDbContext;
        }

        // Displays the login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Handles the login POST request
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginRequest.UserName);
                if (user != null)
                {
                    // Attempts to sign the user in with the provided credentials
                    var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home"); // Redirects to the home page if login is successful
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, "Invalid user credentials"); // Adds error if login fails
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "There is no such user in the system"); // Adds error if the user is not found
                }
            }
            return View(loginRequest); // Returns to the login view with the model if there are errors
        }

        // Handles the logout functionality
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Signs the user out
            return RedirectToAction("Index", "Home"); // Redirects to the home page after logout
        }

        // Displays the registration page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Handles the registration POST request
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(registerRequest.UserName);
                if (user != null)
                {
                    ModelState.AddModelError(String.Empty, "This username is already taken"); // Adds error if the username is already taken
                    return View(registerRequest);
                }

                var role = _context.Roles.FirstOrDefault(r => r.RoleName == CUSTOMER_ROLE); // Fetches the customer role from the database

                var newUser = new User
                {
                    Name = registerRequest.Name,
                    Surname = registerRequest.Surname,
                    UserName = registerRequest.UserName,
                    Password = registerRequest.Password,
                    Role = role! // Assigns the role to the new user
                };

                _context.Users.Add(newUser); // Adds the new user to the database
                await _context.SaveChangesAsync();

                var identityUser = new IdentityUser(registerRequest.UserName); // Creates a new IdentityUser
                var registerResult = await _userManager.CreateAsync(identityUser, registerRequest.Password);

                if (registerResult.Succeeded)
                {
                    // Adds the user to the customer role after successful registration
                    await _userManager.AddToRoleAsync(identityUser, CUSTOMER_ROLE);
                    return RedirectToAction("Login", "Account"); // Redirects to the login page after registration
                }
                else
                {
                    throw new Exception("Unexpected exception while registering user"); // Throws an error if registration fails
                }
            }
            return View(registerRequest); // Returns to the registration view if there are validation errors
        }
    }
}
