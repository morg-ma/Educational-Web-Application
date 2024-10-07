using EducationalWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace EducationalWebApplication.Repository
{
    public interface IInstructorRepository : IRepository<Instructor>
    {
        public IQueryable<Instructor> GetInstructorsWithDepartAndCrs();
        public Instructor GetByIdWithDepartAndCrs(int? id);
        public IQueryable<Instructor> GetAllByName(string name);
    }
}
