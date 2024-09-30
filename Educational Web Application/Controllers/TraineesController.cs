using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using EducationalWebApplication.ViewModels;
using Microsoft.IdentityModel.Tokens;

namespace EducationalWebApplication.Controllers
{
    public class TraineesController : Controller
    {
        private readonly AppDBContext _context;

        public TraineesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Trainees
        public async Task<IActionResult> Index(string sortOrder, string search = "", int pageNo = 1)
        {
            var trainees = _context.Trainees.Include(t => t.Department);

            var traineesVM = await TraineesVM(trainees, sortOrder, search, pageNo);

            return View(traineesVM);
        }

        // GET: Trainees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainees
                .Include(t => t.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainee == null)
            {
                return NotFound();
            }

            return View(trainee);
        }

        // GET: Trainees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "Name");
            return View();
        }

        // POST: Trainees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trainee trainee, IFormFile photo)
        {
            if (!ModelState.IsValid)
            {
                trainee.ImageURL = UploadImage(photo, trainee.Name);
                _context.Add(trainee);
                await _context.SaveChangesAsync();
                TempData["message"] = $"Trainee {trainee.Name} Added Successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "Name", trainee.DepartmentID);
            return View(trainee);
        }

        // GET: Trainees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainee = await _context.Trainees.FindAsync(id);
            if (trainee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "Name", trainee.DepartmentID);
            ViewBag.CurrentImage = trainee.ImageURL;
            return View(trainee);
        }

        // POST: Trainees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ImageURL,Grade,Address,DepartmentID")] Trainee trainee, IFormFile photo, string currentImage)
        {
            if (id != trainee.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    if (photo != null)
                    {
                        trainee.ImageURL = UploadImage(photo, trainee.Name);
                    }
                    else
                    {
                        trainee.ImageURL = currentImage;
                    }
                    _context.Update(trainee);
                    await _context.SaveChangesAsync();
                    TempData["message"] = $"Trainee {trainee.Name} Updated Successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TraineeExists(trainee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "Id", "Name", trainee.DepartmentID);
            return View(trainee);
        }

        // POST: Trainees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainee = await _context.Trainees.FindAsync(id);
            if (trainee != null)
            {
                RemoveStdImage(trainee.ImageURL);
                _context.Trainees.Remove(trainee);
                TempData["message"] = $"Trainee {trainee.Name} Removed Successfully";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TraineeExists(int id)
        {
            return _context.Trainees.Any(e => e.Id == id);
        }

        [NonAction]
        private void RemoveStdImage(string? imageURL)
        {
            if (!string.IsNullOrEmpty(imageURL) && imageURL != "default.png")
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/Trainees", imageURL);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }

        [NonAction]
        private string UploadImage(IFormFile photo, string stdName)
        {
            string insPhoto = "default.png";
            if (photo != null && photo.Length > 0)
            {
                string extension = Path.GetExtension(photo.FileName);
                string photoName = stdName + "_" + DateTime.Now.ToString("yyyyMMddmmss") + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/Trainees", photoName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }

                insPhoto = photoName;
            }
            return insPhoto;
        }

        [NonAction]
        private IQueryable<Trainee> SortColumn(IQueryable<Trainee> trainees, string sortOrder)
        {
            // Sort order parameters for each field
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.GradeSortParm = sortOrder == "Grade" ? "grade_desc" : "Grade";
            ViewBag.AddressSortParm = sortOrder == "Address" ? "address_desc" : "Address";
            ViewBag.DepartmentSortParm = sortOrder == "DepartmentID" ? "dept_desc" : "DepartmentID";
            
            // Sorting logic based on sortOrder parameter
            switch (sortOrder)
            {
                case "Name":
                    trainees = trainees.OrderBy(e => e.Name);
                    break;
                case "name_desc":
                    trainees = trainees.OrderByDescending(e => e.Name);
                    break;
                case "Grade":
                    trainees = trainees.OrderBy(e => e.Grade);
                    break;
                case "salary_desc":
                    trainees = trainees.OrderByDescending(e => e.Grade);
                    break;
                case "Address":
                    trainees = trainees.OrderBy(e => e.Address);
                    break;
                case "address_desc":
                    trainees = trainees.OrderByDescending(e => e.Address);
                    break;
                case "DepartmentID":
                    trainees = trainees.OrderBy(i => i.Department.Name);
                    break;
                case "dept_desc":
                    trainees = trainees.OrderByDescending(i => i.Department.Name);
                    break;
                default:
                    trainees = trainees.OrderBy(e => e.Id); // Default sort by ID
                    break;
            }

            return trainees;
        }

        [NonAction]
        private async Task<TraineesViewModel> TraineesVM(IQueryable<Trainee> trainees, string sortOrder, string search = "", int pageNo = 1)
        {
            // Searching
            if (search != null && search != string.Empty)
            {
                trainees = _context.Trainees
                    .Include(d => d.Department)
                    .Where(i => i.Name.StartsWith(search));
            }

            // Sorting
            trainees = SortColumn(trainees, sortOrder);

            // Pagination
            var page = await PaginatedList<Trainee>.Create(trainees, pageNo, 5);

            var stdVM = new TraineesViewModel();
            stdVM.Trainees = trainees;
            stdVM.Search = search;
            stdVM.SortOrder = sortOrder;
            stdVM.Page = page;
            return stdVM;
        }
    }
}
