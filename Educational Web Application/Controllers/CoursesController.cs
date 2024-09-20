using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
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
        public IActionResult Index()
        {
            return View(_context.Courses.Include(d => d.Department));
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
            if (crs != null)
            {
                if((crs.Name is not null and not "") && (crs.DepartmentID is not 0))
                {
                    TempData["message"] = "Course Saved Successfully!";
                    _context.Courses.Add(crs);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
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
            if ((crs.Name is not null and not "") && (crs.DepartmentID is not 0))
            {
                TempData["message"] = "Course Updated successfully!";
                _context.Courses.Update(crs);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartList = new SelectList(_context.Departments.ToList(), "Id", "Name", crs.DepartmentID);
            return View(crs);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
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

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmedDelete(Course crs)
        {
            TempData["message"] = "Course Deleted Successfully!";
            _context.Courses.Remove(crs);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
