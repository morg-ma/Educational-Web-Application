using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using EducationalWebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EducationalWebApplication.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDBContext _context;

        public CoursesController(AppDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string search = "", int pageNo = 1)
        {
            var courses = _context.Courses.Include(d => d.Department).AsQueryable();

            var crsVM = await CoursesVM(courses, sortOrder, search, pageNo);
            
            return View(crsVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DepartList = new SelectList(_context.Departments.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Id,Name,Degree,MinDegree,Credits,DepartmentID")] Course crs)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = $"Course {crs.Name} Saved Successfully!";
                _context.Courses.Add(crs);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DepartList = new SelectList(_context.Departments.ToList(), "Id", "Name", crs.DepartmentID);
            return View(crs);
        }

        public IActionResult Details(int? id)
        {
            if (id != null)
            {
                var crs = _context.Courses.Include(d => d.Department).FirstOrDefault(c => c.Id == id);
                if (crs != null)
                {
                    return View(crs);
                }
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var crs = _context.Courses.Include(d => d.Department).FirstOrDefault(c => c.Id == id);
                if (crs != null)
                {
                    ViewBag.DepartList = new SelectList(_context.Departments.ToList(), "Id", "Name");
                    return View(crs);
                }
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit([Bind("Id,Name,Degree,MinDegree,Credits,DepartmentID")] Course crs)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = $"Course {crs.Name} Updated successfully!";
                _context.Courses.Update(crs);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartList = new SelectList(_context.Departments.ToList(), "Id", "Name", crs.DepartmentID);
            return View(crs);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmedDelete(Course crs)
        {
            TempData["message"] = $"Course {crs.Name} Deleted Successfully!";
            _context.Courses.Remove(crs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [NonAction]
        private IQueryable<Course> SortColumn(IQueryable<Course> courses, string sortOrder)
        {
            // Sort order parameters for each field
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.DegreeSortParm = sortOrder == "Degree" ? "degree_desc" : "Degree";
            ViewBag.MinDegreeSortParm = sortOrder == "MinDegree" ? "minDegree_desc" : "MinDegree";
            ViewBag.CreditsSortParm = sortOrder == "Credits" ? "credits_desc" : "Credits";
            ViewBag.DepartmentSortParm = sortOrder == "DepartmentID" ? "dept_desc" : "DepartmentID";

            // Sorting logic based on sortOrder parameter
            switch (sortOrder)
            {
                case "Name":
                    courses = courses.OrderBy(e => e.Name);
                    break;
                case "name_desc":
                    courses = courses.OrderByDescending(e => e.Name);
                    break;
                case "Degree":
                    courses = courses.OrderBy(e => e.Degree);
                    break;
                case "degree_desc":
                    courses = courses.OrderByDescending(e => e.Degree);
                    break;
                case "MinDegree":
                    courses = courses.OrderBy(e => e.MinDegree);
                    break;
                case "minDegree_desc":
                    courses = courses.OrderByDescending(e => e.MinDegree);
                    break;
                case "DepartmentID":
                    courses = courses.OrderBy(i => i.Department.Name);
                    break;
                case "Credits":
                    courses = courses.OrderBy(e => e.Credits);
                    break;
                case "credits_desc":
                    courses = courses.OrderByDescending(e => e.Credits);
                    break;
                case "dept_desc":
                    courses = courses.OrderByDescending(i => i.Department.Name);
                    break;
                default:
                    courses = courses.OrderBy(e => e.Id); // Default sort by ID
                    break;
            }

            return courses;
        }

        [NonAction]
        private async Task<CoursesViewModel> CoursesVM(IQueryable<Course> courses, string sortOrder, string search, int pageNo)
        {
            // Searching
            if (search != null && search != string.Empty)
            {
                courses = _context.Courses.Include(d => d.Department).Where(c => c.Name.StartsWith(search));
                ViewBag.search = search;
            }

            // Sorting
            courses = SortColumn(courses, sortOrder);
            ViewBag.sortOrder = sortOrder;

            // Pagination
            var page = await PaginatedList<Course>.Create(courses, pageNo, 4);

            var crsVM = new CoursesViewModel();
            crsVM.Courses = courses;
            crsVM.SortOrder = sortOrder;
            crsVM.Search = search;
            crsVM.Page = page;
            return crsVM;
        }

        private IActionResult CheckDegree(int MinDegree, int Degree)
        {
            if (MinDegree < Degree)
                return Json(true);
            return Json(false);
        }
    }
}
