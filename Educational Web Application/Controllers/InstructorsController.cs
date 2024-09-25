using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using EducationalWebApplication.ViewModels;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;

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
            TempData["message"] = "Instructor Updated successfully!";
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
            TempData["message"] = "Instructor Saved Successfully!";
            return RedirectToAction(nameof(Index));
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
    }
}
