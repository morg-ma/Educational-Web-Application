using Microsoft.AspNetCore.Mvc;

namespace EducationalWebApplication.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {

            return View();
        }
    }
}
