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
using EducationalWebApplication.Repository;
using Microsoft.AspNetCore.Authorization;

namespace EducationalWebApplication.Controllers
{
    [Authorize]
    public class TraineesController : Controller
    {
        private readonly ITraineeRepository _traineeRepo;
        private readonly IDepartmentRepository deptRepo;

        public TraineesController(ITraineeRepository traineeRepo, IDepartmentRepository deptRepo)
        {
            _traineeRepo = traineeRepo;
            this.deptRepo = deptRepo;
        }

        // GET: Trainees
        public async Task<IActionResult> Index(string sortOrder, string search = "", int pageNo = 1)
        {
            var trainees = _traineeRepo.GetAllWithDept();
            var traineesVM = await TraineesVM(trainees, sortOrder, search, pageNo);

            // If the request is an AJAX request, return the partial view only
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_TraineesTablePartial", traineesVM);
            }

            return View(traineesVM);
        }

        // GET: Trainees/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var trainee = await _traineeRepo.GetByIdWithCrs(id);
            return trainee != null? View(trainee) : NotFound();
        }

        // GET: Trainees/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(deptRepo.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Trainees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Trainee trainee, IFormFile? photo)
        {
            if (ModelState.IsValid)
            {
                trainee.ImageURL = UploadImage(photo, trainee.Name);
                _traineeRepo.Add(trainee);
                _traineeRepo.Save();
                TempData["message"] = $"Trainee {trainee.Name} Added Successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(deptRepo.GetAll(), "Id", "Name", trainee.DepartmentID);
            return View(trainee);
        }

        // GET: Trainees/Edit/5
        public IActionResult Edit(int id)
        {
            var trainee = _traineeRepo.GetById(id);
            if (trainee == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(deptRepo.GetAll(), "Id", "Name", trainee.DepartmentID);
            ViewBag.CurrentImage = trainee.ImageURL;
            return View(trainee);
        }

        // POST: Trainees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Trainee trainee, IFormFile? photo, string? currentImage)
        {
            if (ModelState.IsValid)
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
                    _traineeRepo.Update(trainee);
                    _traineeRepo.Save();
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
            ViewData["DepartmentID"] = new SelectList(deptRepo.GetAll(), "Id", "Name", trainee.DepartmentID);
            return View(trainee);
        }

        // POST: Trainees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var trainee = _traineeRepo.GetById(id);
            if (trainee != null)
            {
                RemoveStdImage(trainee.ImageURL);
                _traineeRepo.Delete(id);
                _traineeRepo.Save();
                TempData["message"] = $"Trainee {trainee.Name} Removed Successfully";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TraineeExists(int id)
        {
            return _traineeRepo.GetById(id) != null? true : false;
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
        private string UploadImage(IFormFile? photo, string stdName)
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
            trainees = sortOrder switch
            {
                "Name" => trainees.OrderBy(e => e.Name),
                "name_desc" => trainees.OrderByDescending(e => e.Name),
                "Grade" => trainees.OrderBy(e => e.Grade),
                "salary_desc" => trainees.OrderByDescending(e => e.Grade),
                "Address" => trainees.OrderBy(e => e.Address),
                "address_desc" => trainees.OrderByDescending(e => e.Address),
                "DepartmentID" => trainees.OrderBy(i => i.Department.Name),
                "dept_desc" => trainees.OrderByDescending(i => i.Department.Name),
                _ => trainees.OrderBy(e => e.Id),// Default sort by ID
            };
            return trainees;
        }

        [NonAction]
        private async Task<DataListViewModel<Trainee>> TraineesVM(IQueryable<Trainee> trainees, string sortOrder, string search = "", int pageNo = 1)
        {
            // Searching
            if (search != null && search != string.Empty)
            {
                trainees = _traineeRepo.GetAllByName(search);
            }

            // Sorting
            trainees = SortColumn(trainees, sortOrder);

            // Pagination
            var page = await PaginatedList<Trainee>.Create(trainees, pageNo, 5);

            var stdVM = new DataListViewModel<Trainee>();
            stdVM.ModelList = trainees;
            stdVM.Search = search;
            stdVM.SortOrder = sortOrder;
            stdVM.Page = page;
            return stdVM;
        }
    }
}
