using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using EducationalWebApplication.ViewModels;
using System.Text;

namespace EducationalWebApplication.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly AppDBContext _context;

        public InstructorsController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index(string search = "", int pageNo = 1)
        {
            var instructors = _context.Instructors
                                .Include(d => d.Department)
                                .Include(c => c.Course)
                                .ToList();

            if (search != null && search != string.Empty)
            {
                instructors = _context.Instructors
                                .Include(d => d.Department)
                                .Include(c => c.Course)
                                .Where(i => i.Name.StartsWith(search))
                                .ToList();
                ViewBag.search = search;
            }

            // Pagination
            int noOfRecordsPerPage = 4;
            int noOfPages = Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDouble(instructors.Count) / Convert.ToDouble(noOfRecordsPerPage)
                    )
                );

            int noOfRecordsToSkip = (pageNo - 1) * noOfRecordsPerPage;
            ViewBag.pageNo = pageNo;
            ViewBag.noOfPages = noOfPages;

            instructors = instructors.Skip(noOfRecordsToSkip).Take(noOfRecordsPerPage).ToList();

            return View(instructors);
        }
        public IActionResult Details(int? id)
        {
            if (id != null && id != 0)
            {
                var ins = _context.Instructors
                    .Include(d => d.Department)
                    .Include(c => c.Course)
                    .FirstOrDefault(i => i.Id == id);
                return View(ins);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id != null && id != 0)
            {
                var ins = _context.Instructors
                    .Include(d => d.Department)
                    .Include(c => c.Course)
                    .FirstOrDefault(i => i.Id == id);

                InstWithCoursAndDepartsViewModel vm = new InstWithCoursAndDepartsViewModel();

                vm.Id = ins.Id;
                vm.Name = ins.Name;
                vm.Address = ins.Address;
                vm.ImageURL = ins.ImageURL;
                vm.Salary = ins.Salary;
                vm.CourseID = ins.CourseID;
                vm.DepartmentID = ins.DepartmentID;
                vm.CourseList = _context.Courses.ToList();
                vm.DepartmentList = _context.Departments.ToList();

                return View(vm);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(InstWithCoursAndDepartsViewModel editedIns)
        {
            if (editedIns != null)
            {
                var ins = _context.Instructors
                .Include(d => d.Department)
                .Include(c => c.Course)
                .FirstOrDefault(i => i.Id == editedIns.Id);

                ins.Name = editedIns.Name;
                ins.Address = editedIns.Address;
                ins.ImageURL = editedIns.ImageURL;
                ins.Salary = editedIns.Salary;
                ins.CourseID = editedIns.CourseID;
                ins.DepartmentID = editedIns.DepartmentID;
                ins.Course = _context.Courses.Find(editedIns.CourseID);
                ins.Department = _context.Departments.Find(editedIns.DepartmentID);
                _context.SaveChanges();
                TempData["message"] = "Instructor Updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            editedIns.CourseList = _context.Courses.ToList();
            editedIns.DepartmentList = _context.Departments.ToList();
            return View(editedIns);
        }

        [HttpGet]
        public IActionResult Create()
        {
            InstWithCoursAndDepartsViewModel vm = new InstWithCoursAndDepartsViewModel();
            vm.CourseList = _context.Courses.ToList();
            vm.DepartmentList = _context.Departments.ToList();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(InstWithCoursAndDepartsViewModel editedIns)
        {
            if (editedIns != null)
            {
                var ins = new Instructor();
                ins.Name = editedIns.Name;
                ins.Address = editedIns.Address;
                ins.ImageURL = editedIns.ImageURL;
                ins.Salary = editedIns.Salary;
                ins.CourseID = editedIns.CourseID;
                ins.DepartmentID = editedIns.DepartmentID;
                ins.Course = _context.Courses.Find(editedIns.CourseID);
                ins.Department = _context.Departments.Find(editedIns.DepartmentID);
                _context.Instructors.Add(ins);
                _context.SaveChanges();
                TempData["message"] = "Instructor Saved Successfully!";
                return RedirectToAction(nameof(Index));
            }
            editedIns.CourseList = _context.Courses.ToList();
            editedIns.DepartmentList = _context.Departments.ToList();
            return View(editedIns);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id != null && id != 0)
            {
                var ins = _context.Instructors
                    .Include(d => d.Department)
                    .Include(c => c.Course)
                    .FirstOrDefault(i => i.Id == id);
                return View(ins);
            }
            return NotFound();
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult ConfirmedDelete(Instructor ins)
        {
            _context.Instructors.Remove(ins);
            _context.SaveChanges();
            TempData["message"] = "Instructor Deleted Successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
