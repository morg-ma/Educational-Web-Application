using Microsoft.AspNetCore.Mvc;

namespace EducationalWebApplication.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Error = "";
            TempData["username"] = string.Empty;

            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public IActionResult ConfirmLogin(string username, string password)
        {
            if (username == null || password == null)
            {
                ViewBag.Error = "Must enter your username and password";
                return View();
            }
            if ((username == "admin" && password == "admin") || (username == "Mahmoud" && password == "1234"))
            {
                TempData["username"] = username;
                TempData["message"] = "Logedin successfully!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "The username or password is invalid!";
                return View("Index");
            }
        }
    }
}
