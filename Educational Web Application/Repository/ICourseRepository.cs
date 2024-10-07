using EducationalWebApplication.Models;

namespace EducationalWebApplication.Repository
{
    public interface ICourseRepository : IRepository<Course>
    {
        public IQueryable<Course> GetAllWithDepart();
        public Course GetByIdWithDept(int id);
        public IQueryable<Course> GetAllByName(string name);
        public List<Course> GetCoursesInDepartment(int departmentId);
    }
}