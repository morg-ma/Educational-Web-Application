using EducationalWebApplication.Data;
using EducationalWebApplication.Models;

namespace EducationalWebApplication.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AppDBContext _context;

        public AccountRepository(AppDBContext context)
        {
            _context = context;
        }
        public void Add(User obj)
        {
            _context.Add(obj);
        }
        public void Update(User obj)
        {
            _context.Update(obj);
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            _context.Remove(user);
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public User GetByUsernameAndPassword(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }
    }
}
