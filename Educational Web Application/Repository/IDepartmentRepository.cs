using EducationalWebApplication.Models;

namespace EducationalWebApplication.Repository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        public IQueryable<Department> GetAllByName(string name);
        public IQueryable<Department> GetAllQuery();
        public Department GetDeptWithCourses(int id);
        public Department GetDeptWithInstructors(int id);
        public Department GetDeptWithTrainees(int id);
    }
}
