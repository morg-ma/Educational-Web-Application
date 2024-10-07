using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using EducationalWebApplication.Repository;
using EducationalWebApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;

namespace EducationalWebApplication.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentRepository enrollRepo;

        public EnrollmentController(IEnrollmentRepository enrollRepo)
        {
            this.enrollRepo = enrollRepo;
        }

        // http://localhost:5225/Trainees/Index
        [HttpGet]
        public IActionResult Enroll(int id, [FromServices] ICourseRepository crsRepo, [FromServices] ITraineeRepository traineeRepo, [FromServices] IDepartmentRepository deptRepo)
        {
            var t = traineeRepo.GetById(id);
            
            if (t == null)
            {
                return NotFound();
            }

            EnrollmentViewModel vm = new EnrollmentViewModel();
            vm.TraineeId = t.Id;
            vm.Name = t.Name;
            vm.Grade = t.Grade;
            vm.ImageURL = t.ImageURL;
            vm.Address = t.Address;
            vm.DepartmentID = t.DepartmentID;
            vm.Department = deptRepo.GetById(t.DepartmentID);
            ViewBag.Courses = crsRepo.GetCoursesInDepartment(t.DepartmentID);

            return View(vm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ConfirmEnrollment(int stdId, int crsId)
        {
            var enroll = new CourseResult();
            enroll.TraineeID = stdId;
            enroll.CourseID = crsId;
            enrollRepo.Add(enroll);
            enrollRepo.Save();
            TempData["message"] = "The Enrollment Done Successfully!";
            return RedirectToAction("Index", "Trainees");
        }
    }
}
