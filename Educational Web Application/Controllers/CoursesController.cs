using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using EducationalWebApplication.Repository;
using EducationalWebApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EducationalWebApplication.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICourseRepository crsRepo;
        private readonly IDepartmentRepository deptRepo;
        private readonly IEnrollmentRepository enrollmentRepo;

        //private readonly AppDBContext _context;

        public CoursesController(ICourseRepository crsRepo, IDepartmentRepository deptRepo, IEnrollmentRepository enrollmentRepo) // AppDBContext context)
        {
            // _context = context;
            this.crsRepo = crsRepo;
            this.deptRepo = deptRepo;
            this.enrollmentRepo = enrollmentRepo;
        }
        public async Task<IActionResult> Index(string sortOrder, string search = "", int pageNo = 1)
        {
            var courses = crsRepo.GetAllWithDepart();
            var crsVM = await CoursesVM(courses, sortOrder, search, pageNo);
            
            return View(crsVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DepartList = new SelectList(deptRepo.GetAll(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Course crs)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = $"Course {crs.Name} Saved Successfully!";
                crsRepo.Add(crs);
                crsRepo.Save();
                return RedirectToAction("Index");
            }

            ViewBag.DepartList = new SelectList(deptRepo.GetAll(), "Id", "Name", crs.DepartmentID);
            return View(crs);
        }
        
        public IActionResult CheckDegree(int MinDegree, int Degree)
        {
            if (MinDegree < Degree)
                return Json(true);
            return Json(false);
        }
        
        public IActionResult Details(int id)
        {
            var crs = crsRepo.GetByIdWithDept(id);
            return crs != null? View(crs) : NotFound();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var crs = crsRepo.GetByIdWithDept(id);
            if (crs != null)
            {
                ViewBag.DepartList = new SelectList(deptRepo.GetAll(), "Id", "Name");
                return View(crs);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit([Bind("Id,Name,Degree,MinDegree,Credits,DepartmentID")] Course crs)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = $"Course {crs.Name} Updated successfully!";
                crsRepo.Update(crs);
                crsRepo.Save();
                return RedirectToAction("Index");
            }
            ViewBag.DepartList = new SelectList(deptRepo.GetAll(), "Id", "Name", crs.DepartmentID);
            return View(crs);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmedDelete(Course crs)
        {
            TempData["message"] = $"Course {crs.Name} Deleted Successfully!";
            crsRepo.Delete(crs.Id);
            crsRepo.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ShowTrainees(int crsId)
        {
            // show trainees whoes enrolled the course
            var courseResults = enrollmentRepo.GetTraineesInCourse(crsId);
            ViewBag.CourseResults = courseResults;

            var course = crsRepo.GetByIdWithDept(crsId);
            return course == null ? NotFound() : View(course);
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
            courses = sortOrder switch
            {
                "Name" => courses.OrderBy(e => e.Name),
                "name_desc" => courses.OrderByDescending(e => e.Name),
                "Degree" => courses.OrderBy(e => e.Degree),
                "degree_desc" => courses.OrderByDescending(e => e.Degree),
                "MinDegree" => courses.OrderBy(e => e.MinDegree),
                "minDegree_desc" => courses.OrderByDescending(e => e.MinDegree),
                "DepartmentID" => courses.OrderBy(i => i.Department.Name),
                "Credits" => courses.OrderBy(e => e.Credits),
                "credits_desc" => courses.OrderByDescending(e => e.Credits),
                "dept_desc" => courses.OrderByDescending(i => i.Department.Name),
                _ => courses.OrderBy(e => e.Id),// Default sort by ID
            };
            return courses;
        }

        [NonAction]
        private async Task<DataListViewModel<Course>> CoursesVM(IQueryable<Course> courses, string sortOrder, string search, int pageNo)
        {
            // Searching
            if (search != null && search != string.Empty)
            {
                courses = crsRepo.GetAllByName(search);
                ViewBag.search = search;
            }

            // Sorting
            courses = SortColumn(courses, sortOrder);
            ViewBag.sortOrder = sortOrder;

            // Pagination
            var page = await PaginatedList<Course>.Create(courses, pageNo, 4);

            var crsVM = new DataListViewModel<Course>();
            crsVM.ModelList = courses;
            crsVM.SortOrder = sortOrder;
            crsVM.Search = search;
            crsVM.Page = page;
            return crsVM;
        }

        
    }
}
