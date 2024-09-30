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

namespace EducationalWebApplication.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly AppDBContext _context;

        public DepartmentsController(AppDBContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index(string sortOrder, string search = "", int pageNo = 1)
        {
            var departs = _context.Departments.AsQueryable();

            var deprtVM = await DepartmentsVM(departs, sortOrder, search, pageNo);

            return View(deprtVM);
        }

        // GET: Departments/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department =  _context.Departments
                .FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name,ManagerName")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                _context.SaveChanges();
                TempData["message"] = $"Department {department.Name} Saved Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,ManagerName")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    _context.SaveChanges();
                    TempData["message"] = $"Department {department.Name} Updated Successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
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
            return View(department);
        }

        // GET: Departments/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = _context.Departments
                .FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();    
            }
            _context.Departments.Remove(department);
            _context.SaveChanges();
            TempData["message"] = $"Department {department.Name} Deleted Successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }

        [NonAction]
        private IQueryable<Department> SortColumn(IQueryable<Department> departments, string sortOrder)
        {
            // Sort order parameters for each field
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.ManagerSortParm = sortOrder == "ManagerName" ? "manager_desc" : "ManagerName";

            // Sorting logic based on sortOrder parameter
            switch (sortOrder)
            {
                case "Name":
                    departments = departments.OrderBy(e => e.Name); 
                    break;
                case "name_desc":
                    departments = departments.OrderByDescending(e => e.Name);
                    break;
                case "ManagerName":
                    departments = departments.OrderBy(e => e.ManagerName);
                    break;
                case "manager_desc":
                    departments = departments.OrderByDescending(e => e.ManagerName);
                    break;
                default:
                    departments = departments.OrderBy(e => e.Id); // Default sort by ID
                    break;
            }
            return departments;
        }

        [NonAction]
        private async Task<DepartmentsViewModel> DepartmentsVM(IQueryable<Department> departs, string sortOrder, string search = "", int pageNo = 1)
        {
            // Searching
            if (search != null && search != string.Empty)
            {
                departs = _context.Departments.Where(d => d.Name.StartsWith(search));
                ViewBag.search = search;
            }

            // Sorting
            departs = SortColumn(departs, sortOrder);
            ViewBag.sortOrder = sortOrder;

            // Pagination
            var page = await PaginatedList<Department>.Create(departs, pageNo, 4);

            DepartmentsViewModel viewModel = new DepartmentsViewModel();
            viewModel.Departments = departs;
            viewModel.Search = search;
            viewModel.SortOrder = sortOrder;
            viewModel.Page = page;
            return viewModel;
        }
    }
}
