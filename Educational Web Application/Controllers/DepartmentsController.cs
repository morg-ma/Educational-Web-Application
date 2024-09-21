using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EducationalWebApplication.Data;
using EducationalWebApplication.Models;

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
        public IActionResult Index(string search = "", int pageNo = 1)
        {
            var departs = _context.Departments.ToList();
            if (search != null && search != string.Empty)
            {
                departs = _context.Departments.Where(d => d.Name.StartsWith(search)).ToList();
                ViewBag.search = search;
            }

            // Pagination
            int noOfRecordsPerPage = 4;
            int noOfPages = Convert.ToInt32(
                    Math.Ceiling(
                        Convert.ToDouble(departs.Count) / Convert.ToDouble(noOfRecordsPerPage)
                    )
                );

            int noOfRecordsToSkip = (pageNo - 1) * noOfRecordsPerPage;
            ViewBag.pageNo = pageNo;
            ViewBag.noOfPages = noOfPages;

            departs = departs.Skip(noOfRecordsToSkip).Take(noOfRecordsPerPage).ToList();
            return View(departs);
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
            if (department != null)
            {
                _context.Add(department);
                _context.SaveChanges();
                TempData["message"] = "Department Saved Successfully!";
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

            if (department != null)
            {
                try
                {
                    _context.Update(department);
                    _context.SaveChanges();
                    TempData["message"] = "Department Updated Successfully!";
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
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            _context.SaveChanges();
            TempData["message"] = "Department Deleted Successfully!";
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
