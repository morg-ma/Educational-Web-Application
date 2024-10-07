using EducationalWebApplication.Models;

namespace EducationalWebApplication.Repository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        public IQueryable<Department> GetAllByName(string name);
        public IQueryable<Department> GetAllQuery();
    }
}
