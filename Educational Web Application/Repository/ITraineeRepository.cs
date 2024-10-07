using EducationalWebApplication.Models;

namespace EducationalWebApplication.Repository
{
    public interface ITraineeRepository : IRepository<Trainee>
    {
        public IQueryable<Trainee> GetAllWithDept();
        public Task<Trainee> GetByIdWithCrs(int id);
        public IQueryable<Trainee> GetAllByName(string name);
    }
}
