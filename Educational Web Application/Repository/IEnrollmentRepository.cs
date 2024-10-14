using EducationalWebApplication.Models;

namespace EducationalWebApplication.Repository
{
    public interface IEnrollmentRepository : IRepository<CourseResult>
    {
        public CourseResult GetByIdWithCourseAndTrainee(int id);
        public CourseResult GetByTraineeAndCourseId(int traineeId, int courseId);
        public IEnumerable<Course> GetEnrolledCoursesByTraineeId(int id);
        public List<CourseResult> GetTraineesInCourse(int id);
        
    }
}
