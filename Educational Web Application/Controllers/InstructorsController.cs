using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using EducationalWebApplication.ViewModels;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using NuGet.Common;

namespace EducationalWebApplication.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly AppDBContext _context;

        public InstructorsController(AppDBContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string search = "", int pageNo = 1)
        {
            var instructors = _context.Instructors
                                .Include(d => d.Department)
                                .Include(c => c.Course)
                                .AsQueryable();

            var InsViewModel = await InstructorsVM(instructors, search, sortOrder, pageNo);

            return View(InsViewModel);
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

                if (ins == null)
                    return NotFound();

                ViewBag.CourseList = new SelectList(_context.Courses.ToList(), "Id", "Name");
                ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "Id", "Name");

                ViewBag.CurrentImage = ins.ImageURL;

                return View(ins);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Instructor editedIns, IFormFile photo, string currentImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CourseList = new SelectList(_context.Courses.ToList(), "Id", "Name");
                ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "Id", "Name");
                return View(editedIns);
            }

            if (photo != null)
            {
                editedIns.ImageURL = UploadImage(photo, editedIns.Name);
            }
            else
            {
                editedIns.ImageURL = currentImage;
            }

            _context.Instructors.Update(editedIns);
            _context.SaveChanges();
            TempData["message"] = $"Instructor {editedIns.Name} Updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CourseList = new SelectList(_context.Courses.ToList(), "Id", "Name");
            ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Instructor newIns, IFormFile photo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CourseList = new SelectList(_context.Courses.ToList(), "Id", "Name");
                ViewBag.DepartmentList = new SelectList(_context.Departments.ToList(), "Id", "Name");
                return View(newIns);
            }

            newIns.ImageURL = UploadImage(photo, newIns.Name);
                
            _context.Instructors.Add(newIns);
            _context.SaveChanges();
            TempData["message"] = $"Instructor {newIns.Name} Saved Successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var ins = _context.Instructors.Find(id);
            if (ins == null)
                return NotFound();

            // Delete the photo from the server if it exists
            RemoveInsImage(ins.ImageURL);

            _context.Instructors.Remove(ins);
            _context.SaveChanges();
            TempData["message"] = $"Instructor {ins.Name} Deleted Successfully!";
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        private void RemoveInsImage(string? imageURL)
        {
            if (!string.IsNullOrEmpty(imageURL) && imageURL != "default.png")
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/Instructors", imageURL);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        [NonAction]
        private string UploadImage(IFormFile photo, string insName)
        {
            string insPhoto = "default.png";
            if (photo != null && photo.Length > 0)
            {
                string extension = Path.GetExtension(photo.FileName);
                string photoName = insName + "_" + DateTime.Now.ToString("yyyyMMddmmss") + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/Instructors", photoName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                insPhoto = photoName;
            }
            return insPhoto;
        }

        [NonAction]
        private IQueryable<Instructor> SortColumn(IQueryable<Instructor> instructors, string sortOrder)
        {
            // Sort order parameters for each field
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.SalarySortParm = sortOrder == "Salary" ? "salary_desc" : "Salary";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.DepartmentSortParm = sortOrder == "DepartmentID" ? "dept_desc" : "DepartmentID";
            ViewBag.CourseSortParm = sortOrder == "CourseID" ? "course_desc" : "CourseID";

            // Sorting logic based on sortOrder parameter
            switch (sortOrder)
            {
                case "Name":
                    instructors = instructors.OrderBy(e => e.Name);
                    break;
                case "name_desc":
                    instructors = instructors.OrderByDescending(e => e.Name);
                    break;
                case "Salary":
                    instructors = instructors.OrderBy(e => e.Salary);
                    break;
                case "salary_desc":
                    instructors = instructors.OrderByDescending(e => e.Salary);
                    break;
                case "Address":
                    instructors = instructors.OrderBy(e => e.Address);
                    break;
                case "address_desc":
                    instructors = instructors.OrderByDescending(e => e.Address);
                    break;
                case "DepartmentID":
                    instructors = instructors.OrderBy(i => i.Department.Name);
                    break;
                case "dept_desc":
                    instructors = instructors.OrderByDescending(i => i.Department.Name);
                    break;
                case "CourseID":
                    instructors = instructors.OrderBy(e => e.Course.Name);
                    break;
                case "course_desc":
                    instructors = instructors.OrderByDescending(e => e.Course.Name);
                    break;
                default:
                    instructors = instructors.OrderBy(e => e.Id); // Default sort by ID
                    break;
            }

            return instructors;
        }

        [NonAction]
        private async Task<InstructorsViewModel> InstructorsVM(IQueryable<Instructor> instructors, string sortOrder, string search = "", int pageNo = 1)
        {
            // Searching
            if (search != null && search != string.Empty)
            {
                instructors = _context.Instructors
                    .Include(d => d.Department)
                    .Include(c => c.Course)
                    .Where(i => i.Name.StartsWith(search));
            }

            // Sorting
            instructors = SortColumn(instructors, sortOrder);

            // Pagination
            var page = await PaginatedList<Instructor>.Create(instructors, pageNo, 4);

            var insVM = new InstructorsViewModel();
            insVM.Instructors = instructors;
            insVM.Search = search;
            insVM.SortOrder = sortOrder;
            insVM.Page = page;
            return insVM;
        }
    }
}
