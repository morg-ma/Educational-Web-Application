using EducationalWebApplication.Models;

namespace EducationalWebApplication.Repository
{
    public interface IRepository<T> where T : class
    {
        public void Add(T obj);
        public void Update(T obj);
        public void Delete(int id);
        public List<T> GetAll();
        public T GetById(int id);
        public void Save();
    }
}
