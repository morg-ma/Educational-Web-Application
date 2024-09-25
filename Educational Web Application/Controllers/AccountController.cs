using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using EducationalWebApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationalWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDBContext _context;

        public AccountController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            // Check if the user is already logged in by session
            if (HttpContext.Session.GetString("User") != null)
            {
                // Redirect to home if already logged in
                return RedirectToAction("Index", "Home");
            }

            // Check if there's a "Remember Me" cookie and restore session if available
            if (Request.Cookies["Username"] != null)
            {
                var usernameFromCookie = Request.Cookies["Username"];
                HttpContext.Session.SetString("User", usernameFromCookie);
                return RedirectToAction("Index", "Home");
            }

            // No session or cookie, user needs to log in
            ViewBag.Error = ""; // Only set this when necessary
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel user)
        {
            // Validate username and password input
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                ViewBag.Error = "Must enter your username and password";
                return View();
            }

            // Check credentials from the database
            var searchUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

            if (searchUser == null)
            {
                ViewBag.Error = "The username or password is invalid!";
                return View();
            }

            // Handle "Remember Me" functionality
            if (user.RememberMe)
            {
                // Create a persistent cookie
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7), // Set expiration to 7 days
                    HttpOnly = true // Prevent access from client-side scripts
                };
                Response.Cookies.Append("Username", user.Username, cookieOptions);
            }
            else
            {
                // Clear the "Remember Me" cookie if unchecked
                if (Request.Cookies["Username"] != null)
                {
                    Response.Cookies.Delete("Username");
                }
            }

            // Store the username in the session
            HttpContext.Session.SetString("User", user.Username);

            TempData["Message"] = "Logged In Successfully!";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();

            // Clear the "Remember Me" cookie if it exists
            if (Request.Cookies["Username"] != null)
            {
                Response.Cookies.Delete("Username");
            }

            TempData["Message"] = "Logged Out Successfully!";
            return RedirectToAction(nameof(Login)); // Redirect to the login page
        }

    }
}
