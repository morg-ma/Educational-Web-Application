﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Index()
        {
            return View(_context.Instructors.Include(d => d.Department).Include(c => c.Course).ToList());
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
            string nonce = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            Response.Headers.Add("Content-Security-Policy", $"script-src 'self' 'nonce-{nonce}';");
            ViewBag.Nonce = nonce; // Pass the nonce to your view

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
            string nonce = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            Response.Headers.Add("Content-Security-Policy", $"script-src 'self' 'nonce-{nonce}';");
            ViewBag.Nonce = nonce; // Pass the nonce to your view

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
            string nonce = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            Response.Headers.Add("Content-Security-Policy", $"script-src 'self' 'nonce-{nonce}';");
            ViewBag.Nonce = nonce; // Pass the nonce to your view

            _context.Instructors.Remove(ins);
            _context.SaveChanges();
            TempData["message"] = "Instructor Deleted Successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}