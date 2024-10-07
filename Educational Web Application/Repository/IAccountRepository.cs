using EducationalWebApplication.Models;

namespace EducationalWebApplication.Repository
{
    public interface IAccountRepository : IRepository<User>
    {
        public User GetByUsernameAndPassword(string username, string password);
    }
}
