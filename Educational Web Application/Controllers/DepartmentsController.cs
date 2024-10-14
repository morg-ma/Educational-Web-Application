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
using EducationalWebApplication.Repository;
using Microsoft.AspNetCore.Authorization;

namespace EducationalWebApplication.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentRepository deptRepo;

        //private readonly AppDBContext _context;

        public DepartmentsController(IDepartmentRepository deptRepo)// AppDBContext context)
        {
            // _context = context;
            this.deptRepo = deptRepo;
        }

        // GET: Departments
        public async Task<IActionResult> Index(string sortOrder, string search = "", int pageNo = 1)
        {
            var departs = deptRepo.GetAllQuery();
            var deprtVM = await DepartmentsVM(departs, sortOrder, search, pageNo);

            return View(deprtVM);
        }

        // GET: Departments/Details/5
        public IActionResult Details(int id)
        {
            var department =  deptRepo.GetById(id);
            return department != null? View(department) : NotFound();
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
                deptRepo.Add(department);
                deptRepo.Save();
                TempData["message"] = $"Department {department.Name} Saved Successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public IActionResult Edit(int id)
        {
            var department = deptRepo.GetById(id);
            return department != null ? View(department) : NotFound();
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
                    deptRepo.Update(department);
                    deptRepo.Save();
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
        public IActionResult Delete(int id)
        {
            var department = deptRepo.GetById(id);
            return department != null ? View(department) : NotFound();
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var department = deptRepo.GetById(id);
            if (department == null)
            {
                return NotFound();    
            }
            deptRepo.Delete(id);
            deptRepo.Save();
            TempData["message"] = $"Department {department.Name} Deleted Successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return deptRepo.GetById(id) != null? true : false;
        }

        [NonAction]
        private IQueryable<Department> SortColumn(IQueryable<Department> departments, string sortOrder)
        {
            // Sort order parameters for each field
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.ManagerSortParm = sortOrder == "ManagerName" ? "manager_desc" : "ManagerName";

            // Sorting logic based on sortOrder parameter
            departments = sortOrder switch
            {
                "Name" => departments.OrderBy(e => e.Name),
                "name_desc" => departments.OrderByDescending(e => e.Name),
                "ManagerName" => departments.OrderBy(e => e.ManagerName),
                "manager_desc" => departments.OrderByDescending(e => e.ManagerName),
                _ => departments.OrderBy(e => e.Id),// Default sort by ID
            };
            return departments;
        }

        [NonAction]
        private async Task<DataListViewModel<Department>> DepartmentsVM(IQueryable<Department> departs, string sortOrder, string search = "", int pageNo = 1)
        {
            // Searching
            if (search != null && search != string.Empty)
            {
                departs = deptRepo.GetAllByName(search);
                ViewBag.search = search;
            }

            // Sorting
            departs = SortColumn(departs, sortOrder);
            ViewBag.sortOrder = sortOrder;

            // Pagination
            var page = await PaginatedList<Department>.Create(departs, pageNo, 4);

            var viewModel = new DataListViewModel<Department>();
            viewModel.ModelList = departs;
            viewModel.Search = search;
            viewModel.SortOrder = sortOrder;
            viewModel.Page = page;
            return viewModel;
        }
    }
}
