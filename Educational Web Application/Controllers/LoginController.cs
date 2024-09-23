using EducationalWebApplication.Data;
using EducationalWebApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationalWebApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDBContext _context;

        public LoginController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Error = "";
            TempData["username"] = string.Empty;

            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult ConfirmLogin(LoginViewModel user)
        {
            if (user.Username == null || user.Password == null)
            {
                ViewBag.Error = "Must enter your username and password";
                return View();
            }
            var searchUser = _context.Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            
            if (searchUser == null) {
                ViewBag.Error = "The username or password is invalid!";
                return View("Index");
            }
            
            TempData["CurrentUsername"] = user.Username;
            HttpContext.Session.SetString("User", user.Username);

            TempData["Message"] = "Logged In Successfully!";
            return RedirectToAction("Index", "Home");
        }
    }
}
