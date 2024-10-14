using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using EducationalWebApplication.Repository;
using EducationalWebApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationalWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signIn)
        {
            this.userManager = userManager;
            this.signInManager = signIn;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                // Mapping
                ApplicationUser appUser = new ApplicationUser()
                {
                    UserName = userVM.Username,
                    PasswordHash = userVM.Password
                };

                // Save in Database
                var result = await userManager.CreateAsync(appUser, userVM.Password);
                if (result.Succeeded)
                {
                    //// Assign to Role
                    //var roleResult = await userManager.AddToRoleAsync(appUser, "Admin");
                    //if (roleResult.Succeeded)
                    //{
                    //    // Cookie
                    //    await signInManager.SignInAsync(appUser, isPersistent: false);
                    //    TempData["Message"] = $"Registered Successfully!\nWelcome {userVM.Username}";
                    //    return RedirectToAction("Index", "Home");
                    //}
                    //foreach (var error in roleResult.Errors)
                    //{
                    //    ModelState.AddModelError("", error.Description);
                    //}
                    // Cookie
                    await signInManager.SignInAsync(appUser, isPersistent: false);
                    TempData["Message"] = $"Registered Successfully!\nWelcome {userVM.Username}";
                    return RedirectToAction("Index", "Home");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(userVM);
        }

        [HttpGet]
        public IActionResult Login()
        {
            //// Check if the user is already logged in by session
            //if (HttpContext.Session.GetString("User") != null)
            //{
            //    // Redirect to home if already logged in
            //    return RedirectToAction("Index", "Home");
            //}

            //// Check if there's a "Remember Me" cookie and restore session if available
            //if (Request.Cookies["Username"] != null)
            //{
            //    var usernameFromCookie = Request.Cookies["Username"];
            //    HttpContext.Session.SetString("User", usernameFromCookie);
            //    return RedirectToAction("Index", "Home");
            //}

            //// No session or cookie, user needs to log in
            //ViewBag.Error = ""; // Only set this when necessary
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            // Validate username and password input
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Error", "Must enter your username and password");
                return View();
            }

            // Check credentials from the database
            // var searchUser = new {Username = "admin", Password = "admin"};
            var appUser = await userManager.FindByNameAsync(user.Username);

            if (appUser != null)
            {
                bool checkPass = await userManager.CheckPasswordAsync(appUser, user.Password);
                if (checkPass)
                {
                    // await signIn.SignInAsync(appUser, user.RememberMe);
                    List<Claim> claims = new List<Claim>();
                    //{
                    //    // Add additional claims if you want
                          // new Claim("type", "value");  
                    //};
                    await signInManager.SignInWithClaimsAsync(appUser, user.RememberMe, claims);
                    TempData["Message"] = $"Logged In Successfully!\nWelcome {user.Username}";
                    return RedirectToAction("Index", "Home");
                }
            }
            // ViewBag.Error = "The username or password is incorrect!";
            ModelState.AddModelError("Error", "Username or Password is incorrect!");
            return View(user);
            
            // Before identity
            //// Handle "Remember Me" functionality
            //if (user.RememberMe)
            //{
            //    // Create a persistent cookie
            //    var cookieOptions = new CookieOptions
            //    {
            //        Expires = DateTime.Now.AddDays(7), // Set expiration to 7 days
            //        HttpOnly = true // Prevent access from client-side scripts
            //    };
            //    Response.Cookies.Append("Username", user.Username, cookieOptions);
            //}
            //else
            //{
            //    // Clear the "Remember Me" cookie if unchecked
            //    if (Request.Cookies["Username"] != null)
            //    {
            //        Response.Cookies.Delete("Username");
            //    }
            //}

            //// Store the username in the session
            //HttpContext.Session.SetString("User", user.Username);

            //TempData["Message"] = $"Logged In Successfully!\nWelcome {user.Username}";
            //return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            //// Clear session
            //HttpContext.Session.Clear();

            //// Clear the "Remember Me" cookie if it exists
            //if (Request.Cookies["Username"] != null)
            //{
            //    Response.Cookies.Delete("Username");
            //}

            //TempData["Message"] = "Logged Out Successfully!";
            //return RedirectToAction(nameof(Login)); // Redirect to the login page
            
            // After using Identity
            await signInManager.SignOutAsync();
            TempData["Message"] = "Logged Out Successfully!";
            return RedirectToAction(nameof(Login));
        }

    }
}
