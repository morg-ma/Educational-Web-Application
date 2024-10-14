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
using EducationalWebApplication.Repository;
using Microsoft.AspNetCore.Authorization;

namespace EducationalWebApplication.Controllers
{
    [Authorize]
    public class InstructorsController : Controller
    {
        private readonly IInstructorRepository insRepo;
        private readonly ICourseRepository crsRepo;
        private readonly IDepartmentRepository deptRepo;

        // private readonly AppDBContext _context;

        public InstructorsController(IInstructorRepository insRepo, ICourseRepository crsRepo, IDepartmentRepository deptRepo) // AppDBContext context)
        {
            // _context = context;
            this.insRepo = insRepo;
            this.crsRepo = crsRepo;
            this.deptRepo = deptRepo;
        }
        public async Task<IActionResult> Index(string sortOrder, string search = "", int pageNo = 1)
        {
            var instructors = insRepo.GetInstructorsWithDepartAndCrs();
            var InsViewModel = await InstructorsVM(instructors, search, sortOrder, pageNo);
            return View(InsViewModel);
        }
        public IActionResult Details(int? id)
        {
            var ins = insRepo.GetByIdWithDepartAndCrs(id);
            return ins == null? NotFound() : View(ins);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id != null && id != 0)
            {
                var ins = insRepo.GetByIdWithDepartAndCrs(id);

                if (ins == null)
                    return NotFound();

                ViewBag.CourseList = new SelectList(crsRepo.GetAll(), "Id", "Name");
                ViewBag.DepartmentList = new SelectList(deptRepo.GetAll(), "Id", "Name");

                ViewBag.CurrentImage = ins.ImageURL;

                return View(ins);
            }
            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Instructor editedIns, IFormFile? photo, string? currentImage)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CourseList = new SelectList(crsRepo.GetAll(), "Id", "Name");
                ViewBag.DepartmentList = new SelectList(deptRepo.GetAll(), "Id", "Name");
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

            insRepo.Update(editedIns);
            insRepo.Save();
            TempData["message"] = $"Instructor {editedIns.Name} Updated successfully!";
            
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CourseList = new SelectList(crsRepo.GetAll(), "Id", "Name");
            ViewBag.DepartmentList = new SelectList(deptRepo.GetAll(), "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Instructor newIns, IFormFile? photo)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CourseList = new SelectList(crsRepo.GetAll(), "Id", "Name");
                ViewBag.DepartmentList = new SelectList(deptRepo.GetAll(), "Id", "Name");
                return View(newIns);
            }
            newIns.ImageURL = UploadImage(photo, newIns.Name);
            insRepo.Add(newIns);
            insRepo.Save();
            TempData["message"] = $"Instructor {newIns.Name} Saved Successfully!";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var ins = insRepo.GetById(id);
            if (ins == null)
                return NotFound();

            // Delete the photo from the server if it exists
            RemoveInsImage(ins.ImageURL);
            insRepo.Delete(id);
            insRepo.Save();
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
            instructors = sortOrder switch
            {
                "Name" => instructors.OrderBy(e => e.Name),
                "name_desc" => instructors.OrderByDescending(e => e.Name),
                "Salary" => instructors.OrderBy(e => e.Salary),
                "salary_desc" => instructors.OrderByDescending(e => e.Salary),
                "Address" => instructors.OrderBy(e => e.Address),
                "address_desc" => instructors.OrderByDescending(e => e.Address),
                "DepartmentID" => instructors.OrderBy(i => i.Department.Name),
                "dept_desc" => instructors.OrderByDescending(i => i.Department.Name),
                "CourseID" => instructors.OrderBy(e => e.Course.Name),
                "course_desc" => instructors.OrderByDescending(e => e.Course.Name),
                _ => instructors.OrderBy(e => e.Id),// Default sort by ID
            };
            return instructors;
        }

        [NonAction]
        private async Task<DataListViewModel<Instructor>> InstructorsVM(IQueryable<Instructor> instructors, string sortOrder, string search = "", int pageNo = 1)
        {
            // Searching
            if (search != null && search != string.Empty)
            {
                instructors = insRepo.GetAllByName(search);
            }

            // Sorting
            instructors = SortColumn(instructors, sortOrder);

            // Pagination
            var page = await PaginatedList<Instructor>.Create(instructors, pageNo, 4);

            var insVM = new DataListViewModel<Instructor>();
            insVM.ModelList = instructors;
            insVM.Search = search;
            insVM.SortOrder = sortOrder;
            insVM.Page = page;

            return insVM;
        }
    }
}
