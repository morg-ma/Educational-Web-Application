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

namespace EducationalWebApplication.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly AppDBContext _context;

        public InstructorsController(AppDBContext context)
        {
            _context = context;
        }
        public IActionResult Index(string sortOrder, string search = "", int pageNo = 1)
        {
            var instructors = _context.Instructors
                                .Include(d => d.Department)
                                .Include(c => c.Course)
                                .ToList();

            // Searching
            if (search != null && search != string.Empty)
            {
                instructors = _context.Instructors
                                .Include(d => d.Department)
                                .Include(c => c.Course)
                                .Where(i => i.Name.StartsWith(search))
                                .ToList();
                ViewBag.search = search;
            }

            // Sorting
            instructors = SortColumn(instructors, sortOrder);
            ViewBag.sortOrder = sortOrder;

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
            if (editedIns.Name.IsNullOrEmpty() ||
                editedIns.Salary == 0 ||
                editedIns.Address.IsNullOrEmpty() ||
                editedIns.CourseID == 0 ||
                editedIns.DepartmentID == 0)
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
            if (newIns.Name.IsNullOrEmpty() || 
                newIns.Salary == 0 ||
                newIns.Address.IsNullOrEmpty() ||
                newIns.CourseID == 0 ||
                newIns.DepartmentID == 0)
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
            if (!string.IsNullOrEmpty(ins.ImageURL) && ins.ImageURL != "default.png")
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/Instructors", ins.ImageURL);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Instructors.Remove(ins);
            _context.SaveChanges();
            TempData["message"] = $"Instructor {ins.Name} Deleted Successfully!";
            return RedirectToAction(nameof(Index));
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
        private List<Instructor> SortColumn(IEnumerable<Instructor> instructors, string sortOrder)
        {
            // Sort order parameters for each field
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SalarySortParm = sortOrder == "Salary" ? "salary_desc" : "Salary";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.DepartmentSortParm = sortOrder == "DepartmentID" ? "dept_desc" : "DepartmentID";
            ViewBag.CourseSortParm = sortOrder == "CourseID" ? "course_desc" : "CourseID";

            // Sorting logic based on sortOrder parameter
            switch (sortOrder)
            {
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
                    instructors = instructors.OrderBy(e => e.Name); // Default sort by Name
                    break;
            }

            return instructors.ToList();
        }
    }
}
