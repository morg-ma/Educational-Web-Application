using EducationalWebApplication.Models;
using EducationalWebApplication.Repository;
using EducationalWebApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalWebApplication.Controllers
{
    [Authorize]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentRepository enrollRepo;
        private readonly ICourseRepository crsRepo;
        private readonly ITraineeRepository traineeRepo;
        private readonly IDepartmentRepository deptRepo;

        public EnrollmentController(IEnrollmentRepository enrollRepo, ICourseRepository crsRepo, ITraineeRepository traineeRepo, IDepartmentRepository deptRepo)
        {
            this.enrollRepo = enrollRepo;
            this.crsRepo = crsRepo;
            this.traineeRepo = traineeRepo;
            this.deptRepo = deptRepo;
        }

        // http://localhost:5225/Trainees/Index
        [HttpGet]
        public IActionResult Enrollment(int id)
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
            ViewBag.Courses = crsRepo.GetCoursesInDepartment(t.DepartmentID).Except(enrollRepo.GetEnrolledCoursesByTraineeId(id)).ToList();
            ViewBag.EnrolledCourses = enrollRepo.GetEnrolledCoursesByTraineeId(id);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmUnEnroll(int stdId, int crsId, string from)
        {
            var courseResult = enrollRepo.GetByTraineeAndCourseId(stdId, crsId);
            enrollRepo.Delete(courseResult.Id);
            enrollRepo.Save();
            TempData["message"] = "The Unenrollment Done Successfully!";
            if (from == "Trainee")
            {
                return RedirectToAction("Index", "Trainees");
            }
            return RedirectToAction("Index", "Courses");
        }

        [HttpGet]
        public IActionResult TraineeScore(int courseResultId)
        {
            var cr = enrollRepo.GetByIdWithCourseAndTrainee(courseResultId);
            return cr != null ? View(cr) : NotFound();
        }

        [HttpPost]
        public IActionResult TraineeScore(CourseResult cr)
        {
            var course = crsRepo.GetById(cr.CourseID);
            if (course == null)
            {
                return NotFound();
            }
            if (cr.Score <= course.Degree && cr.Score >= 0)
            {
                enrollRepo.Update(cr);
                enrollRepo.Save();
                TempData["message"] = "The Trainee's Score Saved Successfully!";
                return RedirectToAction("ShowTrainees", "Courses", new { crsId = cr.CourseID });
            }
            ModelState.AddModelError("Score", $"The Score Must be between 0 and {course.Degree}");
            return View(cr);
        }
    }
}
