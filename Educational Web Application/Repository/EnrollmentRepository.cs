using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalWebApplication.Repository
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDBContext _context;

        public EnrollmentRepository(AppDBContext context)
        {
            _context = context;
        }
        public void Add(CourseResult obj)
        {
            _context.Add(obj);
        }
        public void Update(CourseResult obj)
        {
            _context.Update(obj);
        }
        public void Delete(int id)
        {
            var cr = GetById(id);
            _context.Remove(cr);
        }

        public List<CourseResult> GetAll()
        {
            return _context.CourseResults.ToList();
        }

        public CourseResult GetById(int id)
        {
            return _context.CourseResults.Find(id);
        }
        public CourseResult GetByIdWithCourseAndTrainee(int id)
        {
            return _context.CourseResults
                   .Include(c => c.Course)
                   .Include(t => t.Trainee)
                   .FirstOrDefault(cr => cr.Id == id);
        }
        public CourseResult GetByTraineeAndCourseId(int traineeId, int courseId)
        {
            return _context.CourseResults.FirstOrDefault(cr => cr.TraineeID == traineeId && cr.CourseID == courseId);
        }

        public IEnumerable<Course> GetEnrolledCoursesByTraineeId(int id)
        {
            return _context.CourseResults
                .Where(cr => cr.TraineeID ==  id)
                .Include(c => c.Course)
                .ThenInclude(d => d.Department)
                .Select(cr => cr.Course)
                .ToList();
        }

        public List<CourseResult> GetTraineesInCourse(int id)
        {
            var result = _context.CourseResults
                .Where(cr => cr.CourseID == id)
                .Include(t => t.Trainee)
                .ThenInclude(d => d.Department)
                .ToList();
            return result;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
